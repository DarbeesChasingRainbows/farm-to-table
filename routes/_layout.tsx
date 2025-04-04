import { PageProps } from "$fresh/server.ts";

export default function Layout({ Component, state }: PageProps) {
  return (
    <div class="flex flex-col min-h-screen">
      {/* Header */}
      <header class="bg-base-100 shadow-md">
        <div class="navbar container mx-auto">
          <div class="navbar-start">
            <div class="dropdown">
              <label tabIndex={0} class="btn btn-ghost lg:hidden">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h8m-8 6h16" />
                </svg>
              </label>
              <ul tabIndex={0} class="menu menu-sm dropdown-content mt-3 z-[1] p-2 shadow bg-base-100 rounded-box w-52">
                <li><a href="/">Home</a></li>
                <li>
                  <a>Marketplace</a>
                  <ul class="p-2">
                    <li><a href="/marketplace/produce">Produce</a></li>
                    <li><a href="/marketplace/dairy">Dairy</a></li>
                    <li><a href="/marketplace/meat">Meat</a></li>
                  </ul>
                </li>
                <li><a href="/farmers">Farmers</a></li>
                <li><a href="/restaurants">Restaurants</a></li>
                <li><a href="/about">About</a></li>
              </ul>
            </div>
            <a href="/" class="btn btn-ghost normal-case text-xl text-primary">
              <span class="hidden sm:inline">Farm to Table</span>
              <span class="sm:hidden">F2T</span>
            </a>
          </div>
          <div class="navbar-center hidden lg:flex">
            <ul class="menu menu-horizontal px-1">
              <li><a href="/">Home</a></li>
              <li tabIndex={0}>
                <details>
                  <summary>Marketplace</summary>
                  <ul class="p-2 z-10 bg-base-100">
                    <li><a href="/marketplace/produce">Produce</a></li>
                    <li><a href="/marketplace/dairy">Dairy</a></li>
                    <li><a href="/marketplace/meat">Meat</a></li>
                  </ul>
                </details>
              </li>
              <li><a href="/farmers">Farmers</a></li>
              <li><a href="/restaurants">Restaurants</a></li>
              <li><a href="/about">About</a></li>
            </ul>
          </div>
          <div class="navbar-end">
            <a href="/login" class="btn btn-ghost">Login</a>
            <a href="/signup" class="btn btn-primary">Sign Up</a>
          </div>
        </div>
      </header>

      {/* Main Content */}
      <main class="flex-grow">
        <Component />
      </main>

      {/* Footer */}
      <footer class="footer p-10 bg-neutral text-neutral-content">
        <div>
          <span class="footer-title">Farm to Table</span>
          <p class="max-w-md">Connecting local farmers with restaurants for fresher, more sustainable food sourcing.</p>
          <p>&copy; {new Date().getFullYear()} Farm to Table. All rights reserved.</p>
        </div>
        <div>
          <span class="footer-title">Company</span>
          <a href="/about" class="link link-hover">About us</a>
          <a href="/contact" class="link link-hover">Contact</a>
          <a href="/careers" class="link link-hover">Careers</a>
          <a href="/press" class="link link-hover">Press kit</a>
        </div>
        <div>
          <span class="footer-title">Legal</span>
          <a href="/terms" class="link link-hover">Terms of use</a>
          <a href="/privacy" class="link link-hover">Privacy policy</a>
          <a href="/cookies" class="link link-hover">Cookie policy</a>
        </div>
        <div>
          <span class="footer-title">Social</span>
          <div class="grid grid-flow-col gap-4">
            <a><svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" class="fill-current"><path d="M24 4.557c-.883.392-1.832.656-2.828.775 1.017-.609 1.798-1.574 2.165-2.724-.951.564-2.005.974-3.127 1.195-.897-.957-2.178-1.555-3.594-1.555-3.179 0-5.515 2.966-4.797 6.045-4.091-.205-7.719-2.165-10.148-5.144-1.29 2.213-.669 5.108 1.523 6.574-.806-.026-1.566-.247-2.229-.616-.054 2.281 1.581 4.415 3.949 4.89-.693.188-1.452.232-2.224.084.626 1.956 2.444 3.379 4.6 3.419-2.07 1.623-4.678 2.348-7.29 2.04 2.179 1.397 4.768 2.212 7.548 2.212 9.142 0 14.307-7.721 13.995-14.646.962-.695 1.797-1.562 2.457-2.549z"></path></svg></a>
            <a><svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" class="fill-current"><path d="M19.615 3.184c-3.604-.246-11.631-.245-15.23 0-3.897.266-4.356 2.62-4.385 8.816.029 6.185.484 8.549 4.385 8.816 3.6.245 11.626.246 15.23 0 3.897-.266 4.356-2.62 4.385-8.816-.029-6.185-.484-8.549-4.385-8.816zm-10.615 12.816v-8l8 3.993-8 4.007z"></path></svg></a>
            <a><svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" class="fill-current"><path d="M9 8h-3v4h3v12h5v-12h3.642l.358-4h-4v-1.667c0-.955.192-1.333 1.115-1.333h2.885v-5h-3.808c-3.596 0-5.192 1.583-5.192 4.615v3.385z"></path></svg></a>
          </div>
        </div>
      </footer>
    </div>
  );
}