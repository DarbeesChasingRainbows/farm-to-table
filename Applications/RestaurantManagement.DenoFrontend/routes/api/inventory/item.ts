// routes/api/inventory/items.ts
import { HandlerContext } from "$fresh/server.ts";

export const handler = async (req: Request, ctx: HandlerContext): Promise<Response> => {
  const url = new URL(req.url);
  const locationId = url.searchParams.get("locationId");
  const category = url.searchParams.get("category");
  const search = url.searchParams.get("search");
  
  // Build API URL to your backend
  let apiUrl = `${Deno.env.get("API_BASE_URL")}/inventory/items?locationId=${locationId}`;
  if (category) apiUrl += `&category=${category}`;
  if (search) apiUrl += `&search=${search}`;
  
  try {
    // Forward headers for authentication
    const headers = new Headers();
    headers.set("Authorization", req.headers.get("Authorization") || "");
    
    const response = await fetch(apiUrl, { headers });
    
    if (!response.ok) {
      return new Response(JSON.stringify({ error: "Failed to fetch inventory data" }), {
        status: response.status,
        headers: { "Content-Type": "application/json" }
      });
    }
    
    const data = await response.json();
    return new Response(JSON.stringify(data), {
      headers: { "Content-Type": "application/json" }
    });
  } catch (error) {
    return new Response(JSON.stringify({ error: error.message }), {
      status: 500,
      headers: { "Content-Type": "application/json" }
    });
  }
};