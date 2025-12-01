/**
 * Central application configuration.
 * This file exposes an immutable `config` object and some lightweight utilities
 * for building URLs and reading environment variables with default values.
 *
 * Note: preserve comment blocks as required by the project.
 */
export const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

type Environment = 'development' | 'production' | 'test';

export interface AppConfig {
    appName: string;
    environment: Environment;
    apiBaseUrl: string;
    timeoutMs: number;
    featureFlags: Record<string, boolean>;
}

/* Sensible default values for local development */
const DEFAULTS: Partial<AppConfig> = {
    appName: 'nam.client',
    environment: 'development',
    apiBaseUrl: API_BASE_URL,
    timeoutMs: 30_000,
    featureFlags: {},
};

function readEnv(key: string, fallback?: string): string | undefined {
    // Converts the variable name to Vite format (VITE_*)
    const viteKey = key.startsWith('VITE_') ? key : `VITE_${key}`;
    return (import.meta.env[viteKey] as string | undefined) ?? fallback;
}

function parseEnvironment(raw?: string): Environment {
    const val = (raw ?? DEFAULTS.environment) as string;
    if (val === 'production' || val === 'test') return val as Environment;
    return 'development';
}

/**
 * Builds the configuration object at runtime.
 * In production, an error is raised if critical values are missing.
 */
export const config: AppConfig = (() => {
    const environment = parseEnvironment(readEnv('NODE_ENV') ?? readEnv('REACT_APP_NODE_ENV'));

    const apiBaseUrl =
        readEnv('REACT_APP_API_BASE_URL') ??
        readEnv('API_BASE_URL') ??
        DEFAULTS.apiBaseUrl!;

    if (environment === 'production' && !apiBaseUrl) {
        throw new Error('Missing API base URL in production (set API_BASE_URL or REACT_APP_API_BASE_URL).');
    }

    const timeoutMs = Number(readEnv('APP_TIMEOUT_MS') ?? DEFAULTS.timeoutMs) || DEFAULTS.timeoutMs!;
    const appName = readEnv('APP_NAME') ?? DEFAULTS.appName!;
    const featureFlagsRaw = readEnv('FEATURE_FLAGS') ?? '';
    const featureFlags: Record<string, boolean> = {};

    // FORMAT: "flagA=true,flagB=false"
    if (featureFlagsRaw.trim()) {
        for (const entry of featureFlagsRaw.split(',')) {
            const [k, v] = entry.split('=').map(s => s.trim());
            if (!k) continue;
            featureFlags[k] = v ? v.toLowerCase() === 'true' : true;
        }
    }

    const result: AppConfig = {
        appName,
        environment,
        apiBaseUrl,
        timeoutMs,
        featureFlags,
    };

    return Object.freeze(result);
})();

/**
 * Utility: builds an absolute URL to the API ensuring
 * there are no duplicate slashes.
 */
export function buildApiUrl(path: string): string {
    if (!config.apiBaseUrl || config.apiBaseUrl.trim() === '') 
        throw new Error('API base URL is not set. Cannot build API URL.');
    const base = config.apiBaseUrl.replace(/\/+$/, '');
    const suffix = String(path).replace(/^\/+/, '');
    return suffix ? `${base}/${suffix}` : base;
}