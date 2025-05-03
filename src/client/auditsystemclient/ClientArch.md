/src/client/auditsystemclient/src/
├── assets/                  # Static assets like images, fonts, etc.
├── components/              # Reusable UI components
│   ├── common/              # Shared components used across features
│   ├── layout/              # Layout components (header, footer, sidebar)
│   └── features/            # Feature-specific components
├── composables/             # Vue 3 composables (instead of React hooks)
├── views/                   # Page components that correspond to routes
│   ├── audit/               # Audit-related pages
│   ├── compliance/          # Compliance-related pages
│   ├── checklists/          # Checklist-related pages
│   └── user-management/     # User management pages
├── services/                # API service calls and data fetching
│   ├── api/                 # API client configuration
│   ├── audit/               # Audit-related API calls
│   ├── compliance/          # Compliance-related API calls
│   └── auth/                # Authentication services
├── store/                   # State management (Pinia or Vuex)
│   ├── modules/             # Store modules
│   └── composables/         # Custom store composables
├── utils/                   # Utility functions and helpers
├── router/                  # Vue Router configuration
├── styles/                  # Global styles and theme configuration
├── config/                  # Application configuration
└── App.vue                  # Main application component