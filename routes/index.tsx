import { Head } from "$fresh/runtime.ts";
import { Handlers, PageProps } from "$fresh/server.ts";
import { query } from "../db/postgres.ts";

interface HomeData {
  farmCount: number;
  restaurantCount: number;
  productCount: number;
}

export const handler: Handlers<HomeData> = {
  async GET(req, ctx) {
    // In a real app, we would fetch this data from the database
    // For now, we'll use placeholder data
    const data = {
      farmCount: 24,
      restaurantCount: 48,
      productCount: 156,
    };

    return ctx.render(data);
  },
};

export default function Home({ data }: PageProps<HomeData>) {
  return (
    <>
      <Head>
        <title>Farm to Table | Connecting Local Farmers with Restaurants</title>
        <meta name="description" content="A modern platform connecting local farmers with restaurants for fresher, more sustainable food sourcing." />
      </Head>
      <div class="min-h-screen bg-gradient-to-b from-base-100 to-base-200">
        {/* Hero Section */}
        <section class="hero min-h-[70vh] bg-base-200">
          <div class="hero-content text-center">
            <div class="max-w-3xl">
              <h1 class="text-5xl font-bold text-primary">Farm to Table</h1>
              <p class="py-6 text-xl">Connecting local farmers with restaurants for fresher, more sustainable food.</p>
              <div class="flex justify-center gap-4">
                <button class="btn btn-primary">For Farmers</button>
                <button class="btn btn-secondary">For Restaurants</button>
              </div>
            </div>
          </div>
        </section>

        {/* Stats Section */}
        <section class="py-12 bg-base-100">
          <div class="container mx-auto px-4">
            <h2 class="text-3xl font-bold text-center mb-8">Our Growing Community</h2>
            <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
              <div class="stat bg-base-200 rounded-box shadow">
                <div class="stat-figure text-primary">
                  <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" class="inline-block w-8 h-8 stroke-current">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 8h14M5 8a2 2 0 110-4h14a2 2 0 110 4M5 8v10a2 2 0 002 2h10a2 2 0 002-2V8m-9 4h4"></path>
                  </svg>
                </div>
                <div class="stat-title">Local Farms</div>
                <div class="stat-value text-primary">{data.farmCount}</div>
                <div class="stat-desc">Providing fresh produce</div>
              </div>
              
              <div class="stat bg-base-200 rounded-box shadow">
                <div class="stat-figure text-secondary">
                  <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" class="inline-block w-8 h-8 stroke-current">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"></path>
                  </svg>
                </div>
                <div class="stat-title">Restaurants</div>
                <div class="stat-value text-secondary">{data.restaurantCount}</div>
                <div class="stat-desc">Serving farm-fresh meals</div>
              </div>
              
              <div class="stat bg-base-200 rounded-box shadow">
                <div class="stat-figure text-accent">
                  <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" class="inline-block w-8 h-8 stroke-current">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6V4m0 2a2 2 0 100 4m0-4a2 2 0 110 4m-6 8a2 2 0 100-4m0 4a2 2 0 110-4m0 4v2m0-6V4m6 6v10m6-2a2 2 0 100-4m0 4a2 2 0 110-4m0 4v2m0-6V4"></path>
                  </svg>
                </div>
                <div class="stat-title">Products</div>
                <div class="stat-value text-accent">{data.productCount}</div>
                <div class="stat-desc">Locally sourced items</div>
              </div>
            </div>
          </div>
        </section>

        {/* Features Section */}
        <section class="py-12 bg-base-200">
          <div class="container mx-auto px-4">
            <h2 class="text-3xl font-bold text-center mb-8">How It Works</h2>
            <div class="grid grid-cols-1 md:grid-cols-3 gap-8">
              <div class="card bg-base-100 shadow-xl">
                <figure class="px-10 pt-10">
                  <svg xmlns="http://www.w3.org/2000/svg" class="h-20 w-20 text-primary" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.747 0 3.332.477 4.5 1.253v13C19.832 18.477 18.247 18 16.5 18c-1.746 0-3.332.477-4.5 1.253" />
                  </svg>
                </figure>
                <div class="card-body items-center text-center">
                  <h3 class="card-title">For Farmers</h3>
                  <p>List your produce, set prices, and connect directly with local restaurants. Manage your inventory and track orders in real-time.</p>
                </div>
              </div>
              
              <div class="card bg-base-100 shadow-xl">
                <figure class="px-10 pt-10">
                  <svg xmlns="http://www.w3.org/2000/svg" class="h-20 w-20 text-secondary" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" />
                  </svg>
                </figure>
                <div class="card-body items-center text-center">
                  <h3 class="card-title">For Restaurants</h3>
                  <p>Browse local produce, place orders, and manage your farm-to-table inventory. Get notifications when seasonal items become available.</p>
                </div>
              </div>
              
              <div class="card bg-base-100 shadow-xl">
                <figure class="px-10 pt-10">
                  <svg xmlns="http://www.w3.org/2000/svg" class="h-20 w-20 text-accent" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-3 7h3m-3 4h3m-6-4h.01M9 16h.01" />
                  </svg>
                </figure>
                <div class="card-body items-center text-center">
                  <h3 class="card-title">Transparent Supply Chain</h3>
                  <p>Track your food from farm to table. Know exactly where your ingredients come from and when they were harvested.</p>
                </div>
              </div>
            </div>
          </div>
        </section>
      </div>
    </>
  );
}