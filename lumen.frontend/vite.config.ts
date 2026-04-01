import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import tailwindcss from '@tailwindcss/vite'
import { VitePWA } from 'vite-plugin-pwa'

export default defineConfig({
    build: {
        sourcemap: true,
    },
    plugins: [
        vue(),
        tailwindcss(),
        VitePWA({
            registerType: 'autoUpdate',
            includeAssets: ['favicon.ico', 'apple-touch-icon.png', 'masked-icon.svg'],
            manifest: {
                name: 'Lumen: Vitrine Digital',
                short_name: 'Lumen',
                description: 'Sua vitrine inteligente para vendas no WhatsApp',
                theme_color: '#0f172a',
                background_color: '#ffffff',
                display: 'standalone',
                screenshots: [
                    {
                        src: '/screenshot-desktop.png',
                        sizes: '1280x720',
                        type: 'image/png',
                        form_factor: 'wide',
                        label: 'Interface Desktop do Lumen'
                    },
                    {
                        src: '/screenshot-mobile.png',
                        sizes: '750x1334',
                        type: 'image/png',
                        label: 'Interface Mobile do Lumen'
                    }
                ],
                icons: [
                    {
                        src: '/icon-192x192.png',
                        sizes: '192x192',
                        type: 'image/png',
                        purpose: 'any'
                    },
                    {
                        src: '/icon-512x512.png',
                        sizes: '512x512',
                        type: 'image/png',
                        purpose: 'any'
                    },
                    {
                        src: '/icon-512x512.png',
                        sizes: '512x512',
                        type: 'image/png',
                        purpose: 'maskable'
                    }
                ]
            }
        })
    ],
})