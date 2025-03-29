/**
 * BaseEntity abstract class
 * 
 * Base class for all domain entities following DDD principles.
 * Provides common functionality for all entities.
 */

export abstract class BaseEntity {
  private _id: string;
  protected createdAt: Date;
  protected updatedAt: Date;

  constructor(id?: string) {
    this._id = id || crypto.randomUUID();
    this.createdAt = new Date();
    this.updatedAt = new Date();
  }

  get id(): string {
    return this._id;
  }

  get created(): Date {
    return this.createdAt;
  }

  get updated(): Date {
    return this.updatedAt;
  }

  /**
   * Checks if two entities are the same entity by comparing their IDs
   */
  equals(entity?: BaseEntity): boolean {
    if (entity === null || entity === undefined) {
      return false;
    }

    if (this === entity) {
      return true;
    }

    return this._id === entity.id;
  }

  /**
   * Abstract method that all entities must implement to convert to a plain object
   */
  abstract toJSON(): Record<string, unknown>;
}