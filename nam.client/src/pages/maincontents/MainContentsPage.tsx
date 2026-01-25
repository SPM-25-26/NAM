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
};

// New Interfaces for the Map API
type MapMarker = {
  id: string;
  latitude: number;
  longitude: number;
  // We only need coordinates, but you can add name/typology if needed later
};

type MapResponse = {
  name: string;
  marker: MapMarker[];
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
  {
    value: "EatAndDrink",
    label: "Eat&Drink",
    endpoint: "eat-and-drink/card-list",
  },
  { value: "Shopping", label: "Shopping", endpoint: "shopping/card-list" },
  { value: "Service", label: "Service", endpoint: "services/card-list" },
  { value: "Sleep", label: "Sleep", endpoint: "sleep/card-list" },
  { value: "Route", label: "Route", endpoint: "routes/card-list" },
];

// Utility: Haversine formula to calculate distance between two coordinates in km
const calculateDistance = (
  lat1: number,
  lon1: number,
  lat2: number,
  lon2: number,
): number => {
  const R = 6371; // Earth radius in km
  const dLat = (lat2 - lat1) * (Math.PI / 180);
  const dLon = (lon2 - lon1) * (Math.PI / 180);
  const a =
    Math.sin(dLat / 2) * Math.sin(dLat / 2) +
    Math.cos(lat1 * (Math.PI / 180)) *
      Math.cos(lat2 * (Math.PI / 180)) *
      Math.sin(dLon / 2) *
      Math.sin(dLon / 2);
  const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
  return R * c;
};

/**
 * Fetches all coordinates from the map endpoint.
 * Returns a Map object for O(1) access by ID.
 */
const fetchMapCoordinates = async (): Promise<
  Map<string, { lat: number; lon: number }>
> => {
  try {
    const response = await fetch(
      buildApiUrl("map?municipality=Matelica&language=it"),
      {
        method: "GET",
        headers: { Accept: "application/json" },
        credentials: "include",
      },
    );

    if (!response.ok) return new Map();

    const data = (await response.json()) as MapResponse;

    const coordsMap = new Map<string, { lat: number; lon: number }>();

    if (data.marker && Array.isArray(data.marker)) {
      data.marker.forEach((m) => {
        // Ensure lat/lon are valid numbers
        if (m.latitude && m.longitude) {
          coordsMap.set(m.id, { lat: m.latitude, lon: m.longitude });
        }
      });
    }

    return coordsMap;
  } catch (error) {
    console.error("Failed to fetch map coordinates:", error);
    return new Map();
  }
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
  const [selectedCategory, setSelectedCategory] = useState<string | null>(
    "Personalized",
  );
  const [selectedBadge, setSelectedBadge] = useState<string | null>(null);

  // State for distance filtering
  const [selectedDistance, setSelectedDistance] = useState<string | null>(null);

  // Connectivity state
  const [isOffline, setIsOffline] = useState(!navigator.onLine);

  // State for User's GPS location. Defaults to null until permission is granted.
  const [userLocation, setUserLocation] = useState<{
    lat: number;
    lon: number;
  } | null>(null);

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
        ) as unknown as string, // Type assertion if the component strictly expects string, otherwise remove casting
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
        { timeout: 5000, enableHighAccuracy: false }, // 5 seconds max wait
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
            const surveyResponse = await fetch(
              buildApiUrl("user/questionaire-completed"),
              {
                method: "GET",
                credentials: "include",
                headers: { Accept: "application/json" },
              },
            );

            if (surveyResponse.ok) {
              const isComplete = await surveyResponse.json(); // Riceve true o false dal C#

              if (isComplete === false) {
                // Se il questionario � vuoto, reindirizza e FERMA tutto
                navigate("/survey"); // Assicurati che la rotta sia corretta
                return;
              }
            }
          } catch (surveyErr) {
            console.error(
              "Error controlling survey, proceeding anyway:",
              surveyErr,
            );
          }

          setAuthenticated(true);
        } else {
          if (response.status === 401 || response.status === 403) {
            window.location.href = "/login";
          } else {
            console.warn(
              "Server error validating token, staying on page just in case",
            );
            setAuthenticated(true);
          }
        }
      } catch (err) {
        console.error("Auth check error:", err);
        setAuthenticated(true);
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
    category: string,
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
      },
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
      };
    });
  };

  /**
   * Retrieves the list of IDs specifically tailored for the user.
   * This is used to filter the global list when "For You" is selected.
   * Accepts optional Latitude and Longitude.
   */
  const fetchPersonalizedIds = async (
    lat?: number,
    lon?: number,
  ): Promise<string[]> => {
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
        const cleanIds = Array.isArray(rawData)
          ? rawData.map((item: unknown) => {
              if (typeof item === "string") {
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
            })
          : [];

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

        // Fetch User Location first
        const location = await getUserLocation();

        if (location) {
          setUserLocation(location);
        } else {
          setUserLocation(DEFAULT_LOCATION);
        }

        const locationToUse = location || DEFAULT_LOCATION;

        // 1. Fetch all categories
        const categoryPromises = CATEGORY_CONFIGS.map((config) =>
          fetchCategoryData(config.endpoint, config.value).catch((err) => {
            console.error(`Error fetching ${config.value}:`, err);
            return [];
          }),
        );

        // 2. Fetch personalized IDs
        const personalizedIdsPromise = fetchPersonalizedIds(
          locationToUse.lat,
          locationToUse.lon,
        );

        // 3. Fetch Map Coordinates (New)
        const mapCoordinatesPromise = fetchMapCoordinates();

        // Wait for all requests to complete
        const [idsResult, mapData, ...categoryResults] = await Promise.all([
          personalizedIdsPromise,
          mapCoordinatesPromise,
          ...categoryPromises,
        ]);

        // Update the set of personalized IDs
        setPersonalizedIds(idsResult);

        // Flatten results and merge with coordinates immediately
        const allElements = categoryResults.flat().map((item) => {
          const coords = mapData.get(item.id);
          return {
            ...item,
            latitude: coords?.lat, // Undefined if not found, which is fine
            longitude: coords?.lon,
          };
        });

        setElements(allElements);
      } catch (err) {
        console.error("Error while fetching elements:", err);
        setElementsError("Unable to load items. Please try again later.");
      } finally {
        setLoadingElements(false);
      }
    };

    fetchAllElements();
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
    const elementMap = new Map(elements.map((el) => [el.id, el]));

    const missingIds = personalizedIds.filter((id) => !elementMap.has(id));

    if (missingIds.length > 0) {
      console.log("Missing IDs:", missingIds);
      console.groupEnd();
    }

    return personalizedIds
      .map((id) => elementMap.get(id)) // Get object or undefined
      .filter((el): el is ElementItem => el !== undefined) // Remove those not found
      .slice(0, 15); // TAKE ONLY THE FIRST 15 VALID ONES
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
      relevantElements = elements.filter(
        (el) => el.category === selectedCategory,
      );
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
   * Filter elements based on selected category, badge, and distance.
   * Logic updated to handle "Personalized" view and distance calculation.
   */
  const filteredElements = useMemo(() => {
    // 1. Logic for "Personalized" vs Standard Categories
    let baseList: ElementItem[] = [];

    if (selectedCategory === "Personalized") {
      baseList = basePersonalizedList;
    } else if (selectedCategory !== null) {
      baseList = elements.filter((el) => el.category === selectedCategory);
    } else {
      baseList = elements; // Fallback for "All"
    }

    // 2. Filter by Badge
    if (selectedBadge) {
      baseList = baseList.filter((element) => element.badge === selectedBadge);
    }

    // 3. Calculate Distances & Filter
    // Use user location if available, otherwise fallback to default (Matelica)
    const refLat = userLocation?.lat ?? DEFAULT_LOCATION.lat;
    const refLon = userLocation?.lon ?? DEFAULT_LOCATION.lon;

    // Map items to include distance information
    const listWithDistances = baseList.map((item) => {
      if (
        item.latitude !== undefined &&
        item.longitude !== undefined &&
        item.latitude !== 0 &&
        item.longitude !== 0
      ) {
        const dist = calculateDistance(
          refLat,
          refLon,
          item.latitude,
          item.longitude,
        );
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
  }, [
    elements,
    selectedCategory,
    selectedBadge,
    selectedDistance,
    basePersonalizedList,
    userLocation,
  ]);

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
        height: "100vh",
        display: "flex",
        flexDirection: "column",
        backgroundColor: theme.palette.background.default,
      }}
    >
      {/* HEADER STICKY: Offline + AppBar + Filters */}
      <Box sx={{ flexShrink: 0 }}>
        <Container maxWidth="lg" sx={{ pt: 2, pb: 1 }}>
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
            title="Eppoi"
            icon={<FlightIcon sx={{ transform: "rotate(45deg)" }} />}
          />

          {/* Filters */}
          <Box
            sx={{
              display: "flex",
              flexDirection: { xs: "column", sm: "row" },
              gap: 2,
              mt: 2,
              mb: 2,
            }}
          >
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
        </Container>
      </Box>

      {/* SCROLL AREA */}
      <Box sx={{ flex: 1, overflowY: "auto" }}>
        <Container maxWidth="lg" sx={{ pb: 12 }}>
          <Card
            sx={{
              width: "100%",
              boxSizing: "border-box",
              display: "flex",
              flexDirection: "column",
              padding: "2.4rem",
              borderRadius: "1.2rem",
              boxShadow: theme.shadows[3],
              backgroundColor: theme.palette.background.paper,
            }}
          >
            {loadingElements ? (
              <Box
                sx={{
                  display: "flex",
                  justifyContent: "center",
                  alignItems: "center",
                  py: 3,
                }}
              >
                <CircularProgress size={28} />
              </Box>
            ) : elementsError ? (
              <Typography
                variant="body2"
                sx={{ color: theme.palette.error.main }}
              >
                {elementsError}
              </Typography>
            ) : filteredElements.length === 0 ? (
              <Typography
                variant="body2"
                sx={{ color: theme.palette.text.disabled }}
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
                  <ElementCard
                    key={`${item.id}-${item.category}`}
                    title={item.title}
                    badge={item.badge}
                    address={item.address}
                    imageUrl={item.imageUrl}
                    distanceText={item.distanceText}
                    date={item.date}
                    onClick={() =>
                      navigate("/detail-element", {
                        state: {
                          id: item.id,
                          category: stringToCategoryAPI(
                            item.category ?? "sleep",
                          ),
                        },
                      })
                    }
                  />
                ))}
              </Box>
            )}
          </Card>
        </Container>
      </Box>

      {/* BOTTOM NAV */}
      <Box
        sx={{
          flexShrink: 0,
          width: "100%",
        }}
      >
        <SimpleBottomNavigation />
      </Box>
    </Box>
  );
};

export default MainContentsPage;