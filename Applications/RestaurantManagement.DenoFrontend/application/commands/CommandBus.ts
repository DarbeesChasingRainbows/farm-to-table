// application/commands/CommandBus.ts
import { Signal } from "@preact/signals";

export interface Command {
  type: string;
  payload: any;
}

export class CommandBus {
  private handlers = new Map<string, (command: Command) => Promise<void>>();

  register(commandType: string, handler: (command: Command) => Promise<void>) {
    this.handlers.set(commandType, handler);
  }

  async dispatch(command: Command): Promise<void> {
    const handler = this.handlers.get(command.type);
    if (!handler) {
      throw new Error(`No handler registered for command ${command.type}`);
    }
    return await handler(command);
  }
}

// application/queries/QueryBus.ts
export interface Query {
  type: string;
  parameters: any;
}

export class QueryBus {
  private handlers = new Map<string, (query: Query) => Promise<any>>();

  register(queryType: string, handler: (query: Query) => Promise<any>) {
    this.handlers.set(queryType, handler);
  }

  async execute<T>(query: Query): Promise<T> {
    const handler = this.handlers.get(query.type);
    if (!handler) {
      throw new Error(`No handler registered for query ${query.type}`);
    }
    return await handler(query);
  }
}