/**
 * InventoryItem domain model
 * 
 * This represents an inventory item in the system, following DDD principles
 * with proper encapsulation and domain logic.
 */

import { BaseEntity } from "../../shared/models/BaseEntity.ts";

export type InventoryItemStatus = "available" | "reserved" | "sold" | "expired";

export interface InventoryItemProps {
  id?: string;
  name: string;
  description?: string;
  quantity: number;
  unit: string;
  price: number;
  farmerId: string;
  categoryId: string;
  harvestDate: Date;
  expirationDate: Date;
  status?: InventoryItemStatus;
  createdAt?: Date;
  updatedAt?: Date;
}

export class InventoryItem extends BaseEntity {
  private _name: string;
  private _description: string;
  private _quantity: number;
  private _unit: string;
  private _price: number;
  private _farmerId: string;
  private _categoryId: string;
  private _harvestDate: Date;
  private _expirationDate: Date;
  private _status: InventoryItemStatus;

  constructor(props: InventoryItemProps) {
    super(props.id);
    
    this._name = props.name;
    this._description = props.description || "";
    this._quantity = props.quantity;
    this._unit = props.unit;
    this._price = props.price;
    this._farmerId = props.farmerId;
    this._categoryId = props.categoryId;
    this._harvestDate = props.harvestDate;
    this._expirationDate = props.expirationDate;
    this._status = props.status || "available";
    
    this.createdAt = props.createdAt || new Date();
    this.updatedAt = props.updatedAt || new Date();
    
    this.validate();
  }

  // Getters
  get name(): string { return this._name; }
  get description(): string { return this._description; }
  get quantity(): number { return this._quantity; }
  get unit(): string { return this._unit; }
  get price(): number { return this._price; }
  get farmerId(): string { return this._farmerId; }
  get categoryId(): string { return this._categoryId; }
  get harvestDate(): Date { return this._harvestDate; }
  get expirationDate(): Date { return this._expirationDate; }
  get status(): InventoryItemStatus { return this._status; }

  // Domain methods
  updateQuantity(newQuantity: number): void {
    if (newQuantity < 0) {
      throw new Error("Quantity cannot be negative");
    }
    
    this._quantity = newQuantity;
    this.updatedAt = new Date();
    
    if (this._quantity === 0 && this._status === "available") {
      this._status = "sold";
    }
  }

  updatePrice(newPrice: number): void {
    if (newPrice < 0) {
      throw new Error("Price cannot be negative");
    }
    
    this._price = newPrice;
    this.updatedAt = new Date();
  }

  reserve(quantity: number): void {
    if (quantity > this._quantity) {
      throw new Error("Cannot reserve more than available quantity");
    }
    
    if (this._status !== "available") {
      throw new Error(`Cannot reserve item with status: ${this._status}`);
    }
    
    this._quantity -= quantity;
    
    if (this._quantity === 0) {
      this._status = "reserved";
    }
    
    this.updatedAt = new Date();
  }

  markAsSold(): void {
    if (this._status !== "available" && this._status !== "reserved") {
      throw new Error(`Cannot mark as sold an item with status: ${this._status}`);
    }
    
    this._status = "sold";
    this.updatedAt = new Date();
  }

  markAsExpired(): void {
    if (this._status === "sold") {
      throw new Error("Cannot mark as expired an item that has been sold");
    }
    
    this._status = "expired";
    this.updatedAt = new Date();
  }

  isExpired(): boolean {
    return new Date() > this._expirationDate;
  }

  daysUntilExpiration(): number {
    const today = new Date();
    const diffTime = this._expirationDate.getTime() - today.getTime();
    return Math.ceil(diffTime / (1000 * 60 * 60 * 24));
  }

  // Validation
  private validate(): void {
    if (!this._name || this._name.trim().length === 0) {
      throw new Error("Name is required");
    }
    
    if (this._quantity < 0) {
      throw new Error("Quantity cannot be negative");
    }
    
    if (!this._unit || this._unit.trim().length === 0) {
      throw new Error("Unit is required");
    }
    
    if (this._price < 0) {
      throw new Error("Price cannot be negative");
    }
    
    if (!this._farmerId) {
      throw new Error("Farmer ID is required");
    }
    
    if (!this._categoryId) {
      throw new Error("Category ID is required");
    }
    
    if (!this._harvestDate) {
      throw new Error("Harvest date is required");
    }
    
    if (!this._expirationDate) {
      throw new Error("Expiration date is required");
    }
    
    if (this._harvestDate > this._expirationDate) {
      throw new Error("Harvest date cannot be after expiration date");
    }
  }

  // Serialization
  toJSON(): Record<string, unknown> {
    return {
      id: this.id,
      name: this._name,
      description: this._description,
      quantity: this._quantity,
      unit: this._unit,
      price: this._price,
      farmerId: this._farmerId,
      categoryId: this._categoryId,
      harvestDate: this._harvestDate,
      expirationDate: this._expirationDate,
      status: this._status,
      createdAt: this.createdAt,
      updatedAt: this.updatedAt,
    };
  }

  // Factory method
  static create(props: InventoryItemProps): InventoryItem {
    return new InventoryItem(props);
  }
}