import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import { VitePWA } from 'vite-plugin-pwa';

// https://vite.dev/config/
export default defineConfig({
    plugins: [
        react(),
        VitePWA({
            registerType: 'autoUpdate', // Update the app as soon as there is a new version

            // Manifest created manually in /public
            manifest: false,
            devOptions: {
                enabled: true // SW enabled also in npm run dev for testing
            },
            workbox: {
                // 1. Cache of static files (the code of the app)
                globPatterns: ['**/*.{js,css,html,ico,png,svg,json}'],

                cleanupOutdatedCaches: true,

                // 2. Dynamic cache (API e Images)
                runtimeCaching: [
                    {
                        // A. CACHE OF API Calls (Events, Articoles, ArtCulture, Details)
                        // StaleWhileRevalidate: Show cached data, and control if it is changed
                        urlPattern: ({ url }) => url.pathname.includes('card-list') || url.pathname.includes('detail/'),
                        handler: 'StaleWhileRevalidate',
                        options: {
                            cacheName: 'api-data-cache',
                            expiration: {
                                maxEntries: 100, // Save max 100 API responses
                                maxAgeSeconds: 60 * 60 * 24 * 1 // Keep data for 1 day
                            },
                            cacheableResponse: {
                                statuses: [0, 200]
                            }
                        }
                    },
                    {
                        // B. CACHE of external images (image/external)
                        // Show image in the cache, and control if it is changed
                        urlPattern: ({ url }) => url.pathname.includes('image/external'),
                        handler: 'StaleWhileRevalidate',
                        options: {
                            cacheName: 'images-cache',
                            expiration: {
                                maxEntries: 200, // Save max 200 images
                                maxAgeSeconds: 60 * 60 * 24 * 7 // Keep images for 1 week
                            },
                            cacheableResponse: {
                                statuses: [0, 200]
                            }
                        }
                    }
                ]
            }
        })
    ],
});