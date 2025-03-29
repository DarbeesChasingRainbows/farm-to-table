/**
 * PostgreSQL Implementation of Inventory Item Repository
 * 
 * Implements the InventoryItemRepository interface using PostgreSQL
 */

import { InventoryItem, InventoryItemProps, InventoryItemStatus } from "../models/InventoryItem.ts";
import { InventoryItemRepository } from "./InventoryItemRepository.ts";
import { pool, query } from "../../../db/postgres.ts";
import { saveToFirebase, getFromFirebase } from "../../../db/firebase.ts";

export class PostgresInventoryItemRepository implements InventoryItemRepository {
  private readonly tableName = "inventory_items";
  private readonly backupCollection = "inventory_items";

  /**
   * Find inventory item by ID
   */
  async findById(id: string): Promise<InventoryItem | null> {
    try {
      // Try to get from PostgreSQL
      const result = await query(
        `SELECT * FROM ${this.tableName} WHERE id = $1`,
        [id]
      );

      if (result.rows.length > 0) {
        return this.mapToEntity(result.rows[0]);
      }

      // If not found in PostgreSQL, try backup in Firebase
      const backupData = await getFromFirebase(this.backupCollection, id);
      if (backupData) {
        return this.mapToEntity(backupData);
      }

      return null;
    } catch (error) {
      console.error("Error finding inventory item by ID:", error);
      throw error;
    }
  }

  /**
   * Find all inventory items
   */
  async findAll(): Promise<InventoryItem[]> {
    try {
      const result = await query(`SELECT * FROM ${this.tableName}`);
      return result.rows.map(row => this.mapToEntity(row));
    } catch (error) {
      console.error("Error finding all inventory items:", error);
      throw error;
    }
  }

  /**
   * Find inventory items by farmer ID
   */
  async findByFarmerId(farmerId: string): Promise<InventoryItem[]> {
    try {
      const result = await query(
        `SELECT * FROM ${this.tableName} WHERE farmer_id = $1`,
        [farmerId]
      );
      return result.rows.map(row => this.mapToEntity(row));
    } catch (error) {
      console.error("Error finding inventory items by farmer ID:", error);
      throw error;
    }
  }

  /**
   * Find inventory items by category ID
   */
  async findByCategoryId(categoryId: string): Promise<InventoryItem[]> {
    try {
      const result = await query(
        `SELECT * FROM ${this.tableName} WHERE category_id = $1`,
        [categoryId]
      );
      return result.rows.map(row => this.mapToEntity(row));
    } catch (error) {
      console.error("Error finding inventory items by category ID:", error);
      throw error;
    }
  }

  /**
   * Find inventory items that are expiring soon (within days)
   */
  async findExpiringSoon(days: number): Promise<InventoryItem[]> {
    try {
      const result = await query(
        `SELECT * FROM ${this.tableName} 
         WHERE expiration_date <= CURRENT_DATE + INTERVAL '${days} days'
         AND expiration_date >= CURRENT_DATE
         AND status = 'available'`,
      );
      return result.rows.map(row => this.mapToEntity(row));
    } catch (error) {
      console.error("Error finding expiring inventory items:", error);
      throw error;
    }
  }

  /**
   * Find inventory items by status
   */
  async findByStatus(status: string): Promise<InventoryItem[]> {
    try {
      const result = await query(
        `SELECT * FROM ${this.tableName} WHERE status = $1`,
        [status]
      );
      return result.rows.map(row => this.mapToEntity(row));
    } catch (error) {
      console.error("Error finding inventory items by status:", error);
      throw error;
    }
  }

  /**
   * Save an inventory item (create or update)
   */
  async save(item: InventoryItem): Promise<InventoryItem> {
    const client = await pool.connect();
    
    try {
      await client.queryObject("BEGIN");
      
      const existingItem = await this.findById(item.id);
      let result;
      
      if (!existingItem) {
        // Create new item
        result = await client.queryObject(
          `INSERT INTO ${this.tableName} (
            id, name, description, quantity, unit, price, farmer_id, 
            category_id, harvest_date, expiration_date, status, created_at, updated_at
          ) VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9, $10, $11, $12, $13)
          RETURNING *`,
          [
            item.id,
            item.name,
            item.description,
            item.quantity,
            item.unit,
            item.price,
            item.farmerId,
            item.categoryId,
            item.harvestDate,
            item.expirationDate,
            item.status,
            item.created,
            item.updated
          ]
        );
      } else {
        // Update existing item
        result = await client.queryObject(
          `UPDATE ${this.tableName} SET
            name = $2,
            description = $3,
            quantity = $4,
            unit = $5,
            price = $6,
            farmer_id = $7,
            category_id = $8,
            harvest_date = $9,
            expiration_date = $10,
            status = $11,
            updated_at = $12
          WHERE id = $1
          RETURNING *`,
          [
            item.id,
            item.name,
            item.description,
            item.quantity,
            item.unit,
            item.price,
            item.farmerId,
            item.categoryId,
            item.harvestDate,
            item.expirationDate,
            item.status,
            item.updated
          ]
        );
      }
      
      await client.queryObject("COMMIT");
      
      // Backup to Firebase
      await saveToFirebase(this.backupCollection, item.id, item.toJSON());
      
      return this.mapToEntity(result.rows[0]);
    } catch (error) {
      await client.queryObject("ROLLBACK");
      console.error("Error saving inventory item:", error);
      throw error;
    } finally {
      client.release();
    }
  }

  /**
   * Delete an inventory item
   */
  async delete(id: string): Promise<boolean> {
    const client = await pool.connect();
    
    try {
      await client.queryObject("BEGIN");
      
      const result = await client.queryObject(
        `DELETE FROM ${this.tableName} WHERE id = $1 RETURNING id`,
        [id]
      );
      
      await client.queryObject("COMMIT");
      
      return result.rows.length > 0;
    } catch (error) {
      await client.queryObject("ROLLBACK");
      console.error("Error deleting inventory item:", error);
      throw error;
    } finally {
      client.release();
    }
  }

  /**
   * Map database row to domain entity
   */
  private mapToEntity(row: Record<string, any>): InventoryItem {
    const props: InventoryItemProps = {
      id: row.id,
      name: row.name,
      description: row.description,
      quantity: row.quantity,
      unit: row.unit,
      price: row.price,
      farmerId: row.farmer_id,
      categoryId: row.category_id,
      harvestDate: new Date(row.harvest_date),
      expirationDate: new Date(row.expiration_date),
      status: row.status as InventoryItemStatus,
      createdAt: new Date(row.created_at),
      updatedAt: new Date(row.updated_at)
    };

    return new InventoryItem(props);
  }
}