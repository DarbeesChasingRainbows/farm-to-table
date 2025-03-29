/**
 * Inventory Item Repository Interface
 * 
 * Defines the contract for inventory item persistence operations
 * following the Repository pattern in DDD.
 */

import { InventoryItem } from "../models/InventoryItem.ts";

export interface InventoryItemRepository {
  /**
   * Find inventory item by ID
   */
  findById(id: string): Promise<InventoryItem | null>;
  
  /**
   * Find all inventory items
   */
  findAll(): Promise<InventoryItem[]>;
  
  /**
   * Find inventory items by farmer ID
   */
  findByFarmerId(farmerId: string): Promise<InventoryItem[]>;
  
  /**
   * Find inventory items by category ID
   */
  findByCategoryId(categoryId: string): Promise<InventoryItem[]>;
  
  /**
   * Find inventory items that are expiring soon (within days)
   */
  findExpiringSoon(days: number): Promise<InventoryItem[]>;
  
  /**
   * Find inventory items by status
   */
  findByStatus(status: string): Promise<InventoryItem[]>;
  
  /**
   * Save an inventory item (create or update)
   */
  save(item: InventoryItem): Promise<InventoryItem>;
  
  /**
   * Delete an inventory item
   */
  delete(id: string): Promise<boolean>;
}