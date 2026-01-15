import React, { useEffect, useMemo, useState } from "react";
import FlightIcon from "@mui/icons-material/Flight";
import AutoAwesomeIcon from "@mui/icons-material/AutoAwesome"; // Imported for the "For You" icon
import {
    Box,
    Card,
    Container,
    Typography,
    useTheme,
    CircularProgress,
    Alert,
    Fade,
} from "@mui/material";
import WifiOffIcon from "@mui/icons-material/WifiOff"; // Icon for offline state
import { buildApiUrl } from "../../config";
import ElementCard from "../../components/ElementCardComponent";
import CategorySelect from "../../components/SelectComponent";
import type { CategoryOption } from "../../components/SelectComponent";
import { useNavigate } from "react-router-dom";
import { stringToCategoryAPI } from "../detail_element/hooks/IDetailElement";
import MyAppBar from "../../components/appbar";
import SimpleBottomNavigation from "../../components/bottom_bar";

// Constants for default location (Matelica) and filter options
const DEFAULT_LOCATION = { lat: 43.255, lon: 13.0115 };

const DISTANCE_OPTIONS: CategoryOption[] = [
    { value: null, label: "All Distances" },
    { value: "0.5", label: "Within 0.5 km" },
    { value: "1", label: "Within 1 km" },
    { value: "3", label: "Within 3 km" },
    { value: "5", label: "Within 5 km" },
    { value: "10", label: "Within 10 km" },
];

/**
 * Helper to parse coordinates that might use commas or be strings.
 * Ensures we get a valid number or undefined.
 */
const safeParseFloat = (val: string | number | undefined): number | undefined => {
    if (val === undefined || val === null || val === "") return undefined;
    if (typeof val === "number") return val;

    // Replace comma with dot for Italian/European formats (e.g. "43,123" -> "43.123")
    const strVal = val.toString().replace(",", ".");
    const parsed = parseFloat(strVal);

    return isNaN(parsed) ? undefined : parsed;
};

/**
 * Helper to map UI Categories to API endpoints for Details.
 * Matches the user's provided list of categories.
 */
const getApiCategory = (uiCategory: string): string | null => {
    switch (uiCategory) {
        case "ArtCulture": return "art-culture";
        case "Events": return "public-event";
        case "Organization": return "organizations";
        case "Nature": return "nature";
        case "EntertainmentLeisure": return "entertainment-leisure";
        // Articles do not have coordinates, return null to skip fetch
        case "Article": return null;
        default: return null;
    }
};

/**
 * API response shape for card list items
 */
type ApiCardItem = {
    entityId: string;
    entityName: string;
    imagePath: string;
    badgeText: string;
    address: string;
    taxCode?: string;
    date?: string;
};

/**
 * Internal element representation for UI rendering
 */
type ElementItem = {
    id: string;
    title: string;
    badge: string;
    address: string;
    imageUrl?: string;
    date?: string;
    category: string;
    // New properties for location features
    latitude?: number;
    longitude?: number;
    distanceText?: string;
    distanceValue?: number;
    // Flag to avoid re-fetching details
    hydrated?: boolean;
};

/**
 * Category configuration mapping UI categories to API endpoints
 */
type CategoryConfig = {
    value: string;
    label: string;
    endpoint: string;
};

/**
 * Available categories with their corresponding API endpoints
 */
const CATEGORY_CONFIGS: CategoryConfig[] = [
    { value: "Article", label: "Article", endpoint: "article/card-list" },
    {
        value: "ArtCulture",
        label: "ArtCulture",
        endpoint: "art-culture/card-list",
    },
    { value: "Events", label: "Events", endpoint: "public-event/card-list" },
    {
        value: "Organization",
        label: "Organization",
        endpoint: "organizations/card-list",
    },
    { value: "Nature", label: "Nature", endpoint: "nature/card-list" },
    {
        value: "EntertainmentLeisure",
        label: "Entertainment&Leisure",
        endpoint: "entertainment-leisure/card-list",
    },
];

// Utility: Haversine formula to calculate distance between two coordinates in km
const calculateDistance = (lat1: number, lon1: number, lat2: number, lon2: number): number => {
    const R = 6371; // Earth radius in km
    const dLat = (lat2 - lat1) * (Math.PI / 180);
    const dLon = (lon2 - lon1) * (Math.PI / 180);
    const a =
        Math.sin(dLat / 2) * Math.sin(dLat / 2) +
        Math.cos(lat1 * (Math.PI / 180)) * Math.cos(lat2 * (Math.PI / 180)) *
        Math.sin(dLon / 2) * Math.sin(dLon / 2);
    const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
    return R * c;
};

const MainContentsPage: React.FC = () => {
    const theme = useTheme();
    const navigate = useNavigate();
    // Authentication state
    const [loadingAuth, setLoadingAuth] = useState(true);
    const [authenticated, setAuthenticated] = useState(false);

    // Data state
    const [elements, setElements] = useState<ElementItem[]>([]);
    const [loadingElements, setLoadingElements] = useState(false);
    const [elementsError, setElementsError] = useState<string | null>(null);

    // New State: Stores the Set of IDs for the "Personalized" view
    const [personalizedIds, setPersonalizedIds] = useState<string[]>([]);

    // Filter state
    // Changed default state: initialized to "Personalized" instead of null
    const [selectedCategory, setSelectedCategory] = useState<string | null>("Personalized");
    const [selectedBadge, setSelectedBadge] = useState<string | null>(null);

    // State for distance filtering
    const [selectedDistance, setSelectedDistance] = useState<string | null>(null);

    // Connectivity state
    const [isOffline, setIsOffline] = useState(!navigator.onLine);

    // State for User's GPS location. Defaults to null until permission is granted.
    const [userLocation, setUserLocation] = useState<{ lat: number; lon: number } | null>(null);

    /**
     * Category options for the dropdown.
     * "All" has been replaced by "Personalized" (For You) with an icon.
     */
    const categoryOptions: CategoryOption[] = useMemo(() => {
        return [
            {
                value: "Personalized",
                // Rendering the label with the AutoAwesome icon
                label: (
                    <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
                        For You
                        <AutoAwesomeIcon fontSize="small" sx={{ color: "#ffffff" }} />
                    </Box>
                ) as unknown as string // Type assertion if the component strictly expects string, otherwise remove casting
            },
            ...CATEGORY_CONFIGS.map((config) => ({
                value: config.value,
                label: config.label,
            })),
        ];
    }, []);

    /**
     * Helper to get current position wrapped in a Promise.
     * Returns null if denied, not supported, or timed out.
     */
    const getUserLocation = (): Promise<{ lat: number; lon: number } | null> => {
        return new Promise((resolve) => {
            if (!navigator.geolocation) {
                resolve(null);
                return;
            }

            navigator.geolocation.getCurrentPosition(
                (position) => {
                    resolve({
                        lat: position.coords.latitude,
                        lon: position.coords.longitude,
                    });
                },
                (error) => {
                    console.warn("Geolocation access denied or failed:", error.message);
                    resolve(null); // Proceed without location
                },
                { timeout: 5000, enableHighAccuracy: false } // 5 seconds max wait
            );
        });
    };

    /**
     * Monitor online/offline status
     */
    useEffect(() => {
        const handleOnline = () => setIsOffline(false);
        const handleOffline = () => setIsOffline(true);

        window.addEventListener("online", handleOnline);
        window.addEventListener("offline", handleOffline);

        return () => {
            window.removeEventListener("online", handleOnline);
            window.removeEventListener("offline", handleOffline);
        };
    }, []);

    /**
     * Verify user authentication on component mount
     */
    useEffect(() => {
        const checkAuth = async () => {
            // If offline, we assume that the user was logged in the last time.
            if (!navigator.onLine) {
                console.log("Offline mode: skipping auth check");
                setAuthenticated(true);
                setLoadingAuth(false);
                return;
            }
            try {
                // Using auth/validate-token primarily to check valid session/cookies
                const response = await fetch(buildApiUrl("auth/validate-token"), {
                    method: "GET",
                    credentials: "include",
                });

                if (response.ok) {
                    // Survey check if completed
                    try {
                        // Assicurati che l'URL corrisponda alla route del tuo endpoint C#
                        const surveyResponse = await fetch(buildApiUrl("user/questionaire-completed"), {
                            method: "GET",
                            credentials: "include",
                            headers: { "Accept": "application/json" }
                        });

                        if (surveyResponse.ok) {
                            const isComplete = await surveyResponse.json(); // Riceve true o false dal C#

                            if (isComplete === false) {
                                // Se il questionario è vuoto, reindirizza e FERMA tutto
                                navigate("/survey"); // Assicurati che la rotta sia corretta
                                return;
                            }
                        }
                    } catch (surveyErr) {
                        console.error("Error controlling survey, proceeding anyway:", surveyErr);
                    }

                    setAuthenticated(true);
                } else {
                    if (response.status === 401 || response.status === 403) {
                        window.location.href = "/login";
                    } else {
                        console.warn("Server error validating token, staying on page just in case");
                        setAuthenticated(true);
                    }
                }
            } catch (err) {
                console.error("Auth check error:", err);
                window.location.href="/login";
            } finally {
                setLoadingAuth(false);
            }
        };

        checkAuth();
    }, [navigate]);

    /**
     * Fetches card list data from a specific category endpoint
     */
    const fetchCategoryData = async (
        endpoint: string,
        category: string
    ): Promise<ElementItem[]> => {
        // Note: StaleWhileRevalidate strategy in SW will handle caching here automatically
        const response = await fetch(
            buildApiUrl(endpoint + "?municipality=Matelica&language=it"),
            {
                method: "GET",
                headers: {
                    Accept: "application/json",
                },
                credentials: "include",
            }
        );

        if (!response.ok) {
            throw new Error(`Failed to fetch ${category}:  HTTP ${response.status}`);
        }

        const json: unknown = await response.json();
        const data = (json ?? []) as ApiCardItem[];

        // Optimized: We construct the URL string directly instead of fetching blobs.
        // This offloads image loading to the browser/SW and reduces memory usage.
        return data.map((item: ApiCardItem, index: number) => {
            let imageUrl: string | undefined = undefined;

            if (item.imagePath) {
                // Cleaning the path logic preserved from original code
                const cleanedPath = item.imagePath
                    .replace(/-thumb-/g, "-")
                    .replace(/-thumb(?=\.[^.]+$)/, "");

                // Construct the full URL. The img tag will fetch this.
                imageUrl = buildApiUrl("image/external?imagePath=" + cleanedPath);
            }

            return {
                id: item.entityId?.toString() ?? item.taxCode ?? `${category}-${index}`,
                title: item.entityName || "Untitled",
                badge: item.badgeText || "",
                address: item.address || "",
                imageUrl: imageUrl, // Now a string URL, not a blob URL
                date: item.date,
                category: category,
                latitude: undefined, // Will be fetched via Hydration
                longitude: undefined,
                hydrated: false
            };
        });
    };

    /**
     * Retrieves the list of IDs specifically tailored for the user.
     * This is used to filter the global list when "For You" is selected.
     * Accepts optional Latitude and Longitude.
     */
    const fetchPersonalizedIds = async (lat?: number, lon?: number): Promise<string[]> => {
        try {
            // Build the URL with query parameters if coordinates exist
            let endpoint = "user/get-rec";
            if (lat !== undefined && lon !== undefined) {
                endpoint += `?lat=${lat}&lon=${lon}`;
            }

            const response = await fetch(buildApiUrl(endpoint), {
                method: "GET",
                headers: { Accept: "application/json" },
                credentials: "include",
            });

            if (response.ok) {
                const rawData = await response.json();

                // DATA CLEANING:
                const cleanIds = Array.isArray(rawData) ? rawData.map((item: unknown) => {

                    if (typeof item === 'string') {
                        if (item.includes("stringValue")) {
                            try {
                                const parsed = JSON.parse(item);
                                if (parsed.stringValue) return parsed.stringValue;
                            } catch (e) {
                                console.warn("Failed to parse ID JSON:", item, e);
                            }
                        }
                        // If it's a clean string, return it as is
                        return item;
                    }

                    return String(item);
                }) : [];

                return cleanIds;
            }
            return [];
        } catch (error) {
            console.error("Failed to fetch personalized IDs:", error);
            return [];
        }
    };

    /**
     * Fetch data from all category endpoints AND the personalized IDs in parallel
     */
    useEffect(() => {
        if (!authenticated) return;

        const fetchAllElements = async () => {
            try {
                setLoadingElements(true);
                setElementsError(null);

                // Fetch User Location first. 
                // We await this to decide whether to use GPS or fallback to Matelica for recommendations.
                const location = await getUserLocation();

                if (location) {
                    setUserLocation(location);
                } else {
                    setUserLocation(DEFAULT_LOCATION);
                }

                // Determine which location to send to the personalization API
                const locationToUse = location || DEFAULT_LOCATION;

                // Fetch all categories in parallel
                const categoryPromises = CATEGORY_CONFIGS.map((config) =>
                    fetchCategoryData(config.endpoint, config.value).catch((err) => {
                        console.error(`Error fetching ${config.value}:`, err);
                        return []; // Return empty array on error to continue with other categories
                    })
                );

                // Fetch personalized IDs in parallel with categories
                const personalizedIdsPromise = fetchPersonalizedIds(locationToUse.lat, locationToUse.lon);

                // Wait for all requests to complete
                const [idsResult, ...categoryResults] = await Promise.all([
                    personalizedIdsPromise,
                    ...categoryPromises
                ]);

                // Update the set of personalized IDs
                setPersonalizedIds(idsResult);

                // Flatten all results into a single array
                const allElements = categoryResults.flat();
                setElements(allElements);
            } catch (err) {
                console.error("Error while fetching elements:", err);
                setElementsError("Unable to load items. Please try again later.");
            } finally {
                setLoadingElements(false);
            }
        };

        fetchAllElements();

        // Cleanup: No need to revoke object URLs anymore as we use strings
    }, [authenticated]);


    /**
     * Calculate the list of "Top 15" valid elements for the Personalized view.
     * Logic:
     * 1. Map IDs (e.g. 16 IDs) to real objects.
     * 2. Remove those "undefined" (if an ID is not found in the downloaded categories).
     * 3. Trim the list to the first 15.
     * - If all 16 are present -> Show 1-15 (the 16th is excluded).
     * - If the 3rd is missing -> The 16th shifts up one position, enters the top 15, and is shown.
     */
    const basePersonalizedList = useMemo(() => {
        if (selectedCategory !== "Personalized") return [];

        // Map for quick access
        const elementMap = new Map(elements.map(el => [el.id, el]));

        const missingIds = personalizedIds.filter(id => !elementMap.has(id));

        if (missingIds.length > 0) {
            console.log("Missing IDs:", missingIds);
            console.groupEnd();
        }

        return personalizedIds
            .map(id => elementMap.get(id))                  // Get object or undefined
            .filter((el): el is ElementItem => el !== undefined) // Remove those not found
            .slice(0, 15);                                     // TAKE ONLY THE FIRST 15 VALID ONES
    }, [elements, personalizedIds, selectedCategory]);


    /**
     * Extract unique badge options.
     * Updated: When "Personalized", use basePersonalizedList to show only relevant badges.
     */
    const uniqueBadgeOptions: CategoryOption[] = useMemo(() => {
        const badges = new Set<string>();

        let relevantElements: ElementItem[] = [];

        if (selectedCategory === "Personalized") {
            // Use ONLY the 15 calculated elements
            relevantElements = basePersonalizedList;
        } else if (selectedCategory) {
            // Logic standard for other categories
            relevantElements = elements.filter((el) => el.category === selectedCategory);
        } else {
            // Case "All" (if needed)
            relevantElements = elements;
        }

        relevantElements.forEach((el) => {
            if (el.badge) badges.add(el.badge);
        });

        return [
            { value: null, label: "All Badges" },
            ...Array.from(badges)
                .sort()
                .map((b) => ({ value: b, label: b })),
        ];
    }, [elements, selectedCategory, basePersonalizedList]);


    /**
     * Hydrate Elements with Coordinates.
     * Fetches details for loaded items to extract latitude and longitude.
     */
    useEffect(() => {
        if (elements.length === 0 || loadingElements) return;

        // Determine which items need hydration (missing coords & not Article & not hydrated yet)
        const itemsToHydrate = elements.filter(el =>
            !el.hydrated &&
            el.category !== "Article"
        ).slice(0, 50); // Process in batches

        if (itemsToHydrate.length === 0) return;

        const hydrateCoordinates = async () => {

            console.log(`Hydration: Fetching coordinates for ${itemsToHydrate.length} items...`);

            // Map to store found coordinates
            const coordsMap = new Map<string, { lat: number; lon: number }>();

            await Promise.allSettled(itemsToHydrate.map(async (item) => {
                try {
                    const endpointCategory = getApiCategory(item.category);
                    if (!endpointCategory) return;

                    const detailUrl = buildApiUrl(
                        `${endpointCategory}/detail/${item.id}?language=it`
                    );

                    const response = await fetch(detailUrl, {
                        method: "GET",
                        credentials: "include",
                    });

                    if (response.ok) {
                        const data = await response.json();
                        const lat = safeParseFloat(data.latitude);
                        const lon = safeParseFloat(data.longitude);

                        if (lat !== undefined && lon !== undefined) {
                            coordsMap.set(item.id, { lat, lon });
                        }
                    } else {
                        // If we are offline and NOT in cache, we will end up here or in the catch
                        console.warn(`Hydration failed for ${item.id}: HTTP ${response.status}`);
                    }
                } catch (e) {
                    // If we are offline and NOT in cache, the fetch will throw a network error.
                    // We silently ignore it.
                    // console.error(`Hydration error for ${item.id}`, e);
                }
            }));

            // Update state: Add coords and mark processed items as hydrated
            setElements(prevElements => {
                return prevElements.map(el => {
                    if (coordsMap.has(el.id)) {
                        const coords = coordsMap.get(el.id)!;
                        return { ...el, latitude: coords.lat, longitude: coords.lon, hydrated: true };
                    }

                    if (itemsToHydrate.some(i => i.id === el.id)) {
                        return { ...el, hydrated: true };
                    }

                    return el;
                });
            });
            console.log("Hydration: Batch completed.");
        };

        hydrateCoordinates();

    }, [elements, loadingElements]);


    /**
     * Filter elements based on selected category, badge, and distance.
     * Logic updated to handle "Personalized" view and distance calculation.
     */
    const filteredElements = useMemo(() => {
        // 1. Logic for "Personalized" vs Standard Categories
        let baseList: ElementItem[] = [];

        if (selectedCategory === "Personalized") {
            baseList = basePersonalizedList;
        } else if (selectedCategory !== null) {
            baseList = elements.filter(el => el.category === selectedCategory);
        } else {
            baseList = elements; // Fallback for "All"
        }

        // 2. Filter by Badge
        if (selectedBadge) {
            baseList = baseList.filter(element => element.badge === selectedBadge);
        }

        // 3. Calculate Distances & Filter
        // Use user location if available, otherwise fallback to default (Matelica)
        const refLat = userLocation?.lat ?? DEFAULT_LOCATION.lat;
        const refLon = userLocation?.lon ?? DEFAULT_LOCATION.lon;

        // Map items to include distance information
        const listWithDistances = baseList.map((item) => {
            if (item.latitude !== undefined && item.longitude !== undefined && item.latitude !== 0 && item.longitude !== 0) {
                const dist = calculateDistance(refLat, refLon, item.latitude, item.longitude);
                return {
                    ...item,
                    distanceValue: dist,
                    distanceText: `${dist.toFixed(1)} km`,
                };
            }
            return item;
        });

        // Apply Distance Filter
        const maxDistance = selectedDistance ? parseFloat(selectedDistance) : null;

        const distanceFiltered = listWithDistances.filter((item) => {
            // Case A: User selected "All Distances" -> Show everything
            if (maxDistance === null) return true;

            // Case B: Item has NO coordinates -> ALWAYS Show it (as per requirement)
            if (item.distanceValue === undefined) return true;

            // Case C: Item has coordinates -> Show only if within range
            return item.distanceValue <= maxDistance;
        });

        // 4. Deduplication
        const uniqueIds = new Set();
        const distinctElements: ElementItem[] = [];

        for (const item of distanceFiltered) {
            if (!uniqueIds.has(item.id)) {
                uniqueIds.add(item.id);
                distinctElements.push(item);
            }
        }

        return distinctElements;
    }, [elements, selectedCategory, selectedBadge, selectedDistance, basePersonalizedList, userLocation]);

    /**
     * Reset badge filter when category changes
     */
    useEffect(() => {
        setSelectedBadge(null);
    }, [selectedCategory]);

    // Show loader during authentication check
    if (loadingAuth) {
        return (
            <Box
                sx={{
                    backgroundColor: theme.palette.background.default,
                    minHeight: "100vh",
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "center",
                }}
            >
                <CircularProgress />
            </Box>
        );
    }

    // Prevent rendering if not authenticated
    if (!authenticated) {
        return null;
    }

    return (
        <Box
            sx={{
                backgroundColor: theme.palette.background.default,
                minHeight: "100vh",
            }}
        >
            <Container maxWidth="lg">
                <Box
                    sx={{
                        display: "flex",
                        flexDirection: "column",
                        alignItems: "center",
                        paddingTop: 1,
                        paddingBottom: 4,
                    }}
                >
                    {/* OFFLINE BANNER */}
                    <Fade in={isOffline} unmountOnExit>
                        <Alert
                            severity="warning"
                            icon={<WifiOffIcon fontSize="inherit" />}
                            sx={{ width: "100%", mb: 2, borderRadius: 2 }}
                        >
                            You are currently offline. Viewing cached content.
                        </Alert>
                    </Fade>

                    <MyAppBar
                        title={"Eppoi"}
                        icon={<FlightIcon sx={{ transform: "rotate(45deg)" }} />}
                    />
                    {/* Main content card */}
                    <Card
                        sx={{
                            width: "100%",
                            padding: "2.4rem",
                            borderRadius: "1.2rem",
                            boxShadow: theme.shadows[3],
                            backgroundColor: theme.palette.background.paper,
                        }}
                    >
                        {/* Filter controls */}
                        <Box
                            sx={{
                                display: "flex",
                                flexDirection: { xs: "column", sm: "row" },
                                gap: 2,
                            }}
                        >
                            {/* Category filter */}
                            <Box sx={{ flex: 1, minWidth: 0 }}>
                                <CategorySelect
                                    label="Category"
                                    value={selectedCategory}
                                    options={categoryOptions}
                                    onChange={(val) => setSelectedCategory(val)}
                                />
                            </Box>
                            {/* Badge filter */}
                            <Box sx={{ flex: 1, minWidth: 0 }}>
                                <CategorySelect
                                    label="Badge"
                                    value={selectedBadge}
                                    options={uniqueBadgeOptions}
                                    onChange={(val) => setSelectedBadge(val)}
                                    accentColor="linear-gradient(90deg, rgb(138, 174, 254) 0%, rgb(204, 136, 253) 100%)"
                                />
                            </Box>
                            {/* Distance filter */}
                            <Box sx={{ flex: 1, minWidth: 0 }}>
                                <CategorySelect
                                    label="Distance"
                                    value={selectedDistance}
                                    options={DISTANCE_OPTIONS}
                                    onChange={(val) => setSelectedDistance(val)}
                                    accentColor="#9810fa"
                                />
                            </Box>
                        </Box>

                        {/* Content area */}
                        {loadingElements ? (
                            <Box
                                sx={{
                                    display: "flex",
                                    justifyContent: "center",
                                    alignItems: "center",
                                    paddingY: 2,
                                }}
                            >
                                <CircularProgress size={28} />
                            </Box>
                        ) : elementsError ? (
                            <Typography
                                variant="body2"
                                sx={{ color: theme.palette.error.main, mt: 2 }}
                            >
                                {elementsError}
                            </Typography>
                        ) : filteredElements.length === 0 ? (
                            <Typography
                                variant="body2"
                                sx={{
                                    color: theme.palette.text.disabled,
                                    mt: 2,
                                }}
                            >
                                {selectedCategory === "Personalized"
                                    ? "No personalized suggestions available at the moment."
                                    : "No items available for the selected filters."}
                            </Typography>
                        ) : (
                            <Box
                                sx={{
                                    display: "grid",
                                    gridTemplateColumns: {
                                        xs: "1fr",
                                        sm: "repeat(2, minmax(0, 1fr))",
                                        md: "repeat(3, minmax(0, 1fr))",
                                        lg: "repeat(3, minmax(0, 1fr))",
                                    },
                                    gridAutoRows: "1fr",
                                    gap: 2.5,
                                    marginTop: 2,
                                }}
                            >
                                {filteredElements.map((item) => (
                                    <Box
                                        key={`${item.id}-${item.category}`}
                                        sx={{ height: "100%" }}
                                    >
                                        <ElementCard
                                            title={item.title}
                                            badge={item.badge}
                                            address={item.address}
                                            // Passing the formatted distance string to the card
                                            distanceText={item.distanceText}
                                            imageUrl={item.imageUrl} // Note: ElementCard should handle string URL now
                                            date={item.date}
                                            onClick={() => {
                                                navigate("/detail-element", {
                                                    state: {
                                                        //TODO: enable mock view
                                                        // id: "mock_test",
                                                        id: item.id,
                                                        category: stringToCategoryAPI(
                                                            item.category ?? "sleep"
                                                        ),
                                                    },
                                                });
                                            }}
                                        />
                                    </Box>
                                ))}
                            </Box>
                        )}
                    </Card>
                </Box>
            </Container>
            <SimpleBottomNavigation />
        </Box>
    );
};

export default MainContentsPage;