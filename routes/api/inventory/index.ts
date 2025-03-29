/**
 * Inventory API Route Handler
 * 
 * Handles API requests for inventory items
 */

import { Handlers } from "$fresh/server.ts";
import { PostgresInventoryItemRepository } from "../../../domains/inventory/repositories/PostgresInventoryItemRepository.ts";
import { InventoryItem } from "../../../domains/inventory/models/InventoryItem.ts";

// Initialize repository
const inventoryRepository = new PostgresInventoryItemRepository();

export const handler: Handlers = {
  /**
   * GET /api/inventory
   * Get all inventory items or filter by query parameters
   */
  async GET(req) {
    try {
      const url = new URL(req.url);
      const farmerId = url.searchParams.get("farmerId");
      const categoryId = url.searchParams.get("categoryId");
      const status = url.searchParams.get("status");
      const expiringSoon = url.searchParams.get("expiringSoon");
      
      let items: InventoryItem[] = [];
      
      if (farmerId) {
        items = await inventoryRepository.findByFarmerId(farmerId);
      } else if (categoryId) {
        items = await inventoryRepository.findByCategoryId(categoryId);
      } else if (status) {
        items = await inventoryRepository.findByStatus(status);
      } else if (expiringSoon) {
        const days = parseInt(expiringSoon, 10) || 7; // Default to 7 days
        items = await inventoryRepository.findExpiringSoon(days);
      } else {
        items = await inventoryRepository.findAll();
      }
      
      return new Response(JSON.stringify({
        success: true,
        data: items.map(item => item.toJSON()),
        count: items.length
      }), {
        headers: { "Content-Type": "application/json" }
      });
    } catch (error) {
      console.error("Error handling GET request:", error);
      return new Response(JSON.stringify({
        success: false,
        error: error.message
      }), {
        status: 500,
        headers: { "Content-Type": "application/json" }
      });
    }
  },
  
  /**
   * POST /api/inventory
   * Create a new inventory item
   */
  async POST(req) {
    try {
      const body = await req.json();
      
      // Create domain entity
      const inventoryItem = InventoryItem.create({
        name: body.name,
        description: body.description,
        quantity: parseFloat(body.quantity),
        unit: body.unit,
        price: parseFloat(body.price),
        farmerId: body.farmerId,
        categoryId: body.categoryId,
        harvestDate: new Date(body.harvestDate),
        expirationDate: new Date(body.expirationDate)
      });
      
      // Save to repository
      const savedItem = await inventoryRepository.save(inventoryItem);
      
      return new Response(JSON.stringify({
        success: true,
        data: savedItem.toJSON()
      }), {
        status: 201,
        headers: { "Content-Type": "application/json" }
      });
    } catch (error) {
      console.error("Error handling POST request:", error);
      return new Response(JSON.stringify({
        success: false,
        error: error.message
      }), {
        status: 400,
        headers: { "Content-Type": "application/json" }
      });
    }
  }
};