import { Pool } from "postgres";

// Initialize PostgreSQL connection pool
const POOL_CONNECTIONS = 10;

// Get database connection details from environment variables
const dbUrl = Deno.env.get("DATABASE_URL");

if (!dbUrl) {
  console.error("DATABASE_URL environment variable is not set");
}

// Create a connection pool
export const pool = new Pool(dbUrl, POOL_CONNECTIONS, true);

// Test the connection
const connectionTest = async () => {
  try {
    const client = await pool.connect();
    const result = await client.queryObject`SELECT version()`;
    console.log("PostgreSQL connection successful:");
    console.log(result.rows[0]);
    client.release();
  } catch (err) {
    console.error("PostgreSQL connection error:", err);
  }
};

// Run connection test when this module is imported
connectionTest();

// Helper function to execute queries
export async function query(sql: string, params: unknown[] = []) {
  const client = await pool.connect();
  try {
    return await client.queryObject(sql, params);
  } finally {
    client.release();
  }
}

// Graceful shutdown
Deno.addSignalListener("SIGINT", () => {
  console.log("Closing PostgreSQL pool connections...");
  pool.end();
  Deno.exit();
});

Deno.addSignalListener("SIGTERM", () => {
  console.log("Closing PostgreSQL pool connections...");
  pool.end();
  Deno.exit();
});