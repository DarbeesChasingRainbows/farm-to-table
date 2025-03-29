# Farm to Table

A modern farm-to-table application connecting local farmers with restaurants, built with Deno Fresh, PostgreSQL, and Tailwind CSS.

## Tech Stack

- **Frontend**: Deno Fresh with Islands Architecture, Tailwind CSS, and DaisyUI
- **Backend**: Deno Fresh server-side rendering and API routes
- **Primary Database**: PostgreSQL 17
- **Backup Database**: Firebase
- **Architecture**: Domain-Driven Design (DDD)

## Features

- Farmer marketplace for listing produce and products
- Restaurant inventory management
- Order processing and tracking
- Seasonal produce calendar
- Analytics dashboard
- User authentication and authorization

## Project Structure

The project follows Domain-Driven Design principles:

```
farm-to-table/
├── components/        # Shared UI components
├── domains/           # Domain-specific modules
│   ├── farmers/       # Farmer domain
│   ├── restaurants/   # Restaurant domain
│   ├── inventory/     # Inventory domain
│   ├── orders/        # Order domain
│   └── users/         # User domain
├── islands/           # Interactive UI components
├── routes/            # Application routes
├── static/            # Static assets
├── utils/             # Utility functions
├── db/                # Database connections and models
├── services/          # Shared services
├── main.ts            # Entry point
├── deno.json          # Deno configuration
└── fresh.config.ts    # Fresh framework configuration
```

## Getting Started

### Prerequisites

- [Deno](https://deno.land/) v1.38 or higher
- [PostgreSQL](https://www.postgresql.org/) v17
- [Firebase](https://firebase.google.com/) account (for backup database)

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/DarbeesChasingRainbows/farm-to-table.git
   cd farm-to-table
   ```

2. Create a `.env` file with your database configuration:
   ```
   DATABASE_URL=postgres://username:password@localhost:5432/farmtotable
   FIREBASE_API_KEY=your_firebase_api_key
   FIREBASE_AUTH_DOMAIN=your_firebase_auth_domain
   FIREBASE_PROJECT_ID=your_firebase_project_id
   ```

3. Start the development server:
   ```bash
   deno task start
   ```

4. Open [http://localhost:8000](http://localhost:8000) in your browser.

## Development

### Commands

- `deno task start` - Start the development server
- `deno task build` - Build the production version
- `deno task preview` - Preview the production build
- `deno task test` - Run tests

## License

MIT