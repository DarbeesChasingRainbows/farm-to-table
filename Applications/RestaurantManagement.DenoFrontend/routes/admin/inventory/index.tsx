// routes/admin/inventory/index.tsx
import { Head } from "$fresh/runtime.ts";
import InventoryManagement from "../../../islands/admin/InventoryManagement.tsx";

export default function InventoryPage() {
  return (
    <>
      <Head>
        <title>Inventory Management</title>
      </Head>
      <InventoryManagement />
    </>
  );
}