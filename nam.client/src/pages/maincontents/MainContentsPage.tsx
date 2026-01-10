import React, { useEffect, useMemo, useState } from "react";
import FlightIcon from "@mui/icons-material/Flight";
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

  // Filter state
  const [selectedCategory, setSelectedCategory] = useState<string | null>(null);
  const [selectedBadge, setSelectedBadge] = useState<string | null>(null);

  // Connectivity state
  const [isOffline, setIsOffline] = useState(!navigator.onLine);

  /**
   * Category options for the dropdown, including "All" option
   */
  const categoryOptions: CategoryOption[] = useMemo(() => {
    return [
      { value: null, label: "All" },
      ...CATEGORY_CONFIGS.map((config) => ({
        value: config.value,
        label: config.label,
      })),
    ];
  }, []);

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
          setAuthenticated(true);
        } else {
          window.location.href = "/login";
        }
      } catch (err) {
        console.error("Auth check error:", err);
        window.location.href = "/login";
      } finally {
        setLoadingAuth(false);
      }
    };

    checkAuth();
  }, []);

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
      };
    });
  };

  /**
   * Fetch data from all category endpoints in parallel
   */
  useEffect(() => {
    if (!authenticated) return;

    const fetchAllElements = async () => {
      try {
        setLoadingElements(true);
        setElementsError(null);

        // Fetch all categories in parallel
        const categoryPromises = CATEGORY_CONFIGS.map((config) =>
          fetchCategoryData(config.endpoint, config.value).catch((err) => {
            console.error(`Error fetching ${config.value}:`, err);
            return []; // Return empty array on error to continue with other categories
          })
        );

        const results = await Promise.all(categoryPromises);

        // Flatten all results into a single array
        const allElements = results.flat();
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
   * Handle user logout
   */
  const handleLogout = async () => {
    try {
      const response = await fetch(buildApiUrl("auth/logout"), {
        method: "POST",
        credentials: "include",
      });

      if (!response.ok) {
        console.error("Logout failed");
      }
    } catch (err) {
      console.error("Logout error:", err);
    } finally {
      window.location.href = "/login";
    }
  };

  /**
   * Extract unique badge options from fetched elements
   * Filtered by selected category if applicable
   */
  const uniqueBadgeOptions: CategoryOption[] = useMemo(() => {
    const badges = new Set<string>();

    // Only consider elements from the selected category
    const relevantElements = selectedCategory
      ? elements.filter((el) => el.category === selectedCategory)
      : elements;

    relevantElements.forEach((el) => {
      if (el.badge) badges.add(el.badge);
    });

    return [
      { value: null, label: "All Badges" },
      ...Array.from(badges)
        .sort()
        .map((b) => ({ value: b, label: b })),
    ];
  }, [elements, selectedCategory]);

  useEffect(() => {
    if (elements.length === 0 || loadingElements) return;

    const prefetchDetails = async () => {
      // Do only if we are Online, otherwise it's useless
      if (!navigator.onLine) return;

      console.log("Prefetch: Inizio scaricamento dettagli in background...");

      // Optimization: Prefetch only the first 40 items to save bandwidth
      // The Service Worker will cache these responses.
      const itemsToPrefetch = elements.slice(0, 40);

      for (const item of itemsToPrefetch) {
        try {
          let endpointCategory = "";
          // Mappa Categoria UI -> Path API
          switch (item.category) {
            case "Article":
              endpointCategory = "article";
              break;
            case "ArtCulture":
              endpointCategory = "art-culture";
              break;
            case "Events":
              endpointCategory = "public-event";
              break;
            case "Organization":
              endpointCategory = "organizations";
              break;
            case "Nature":
              endpointCategory = "nature";
              break;
            case "EntertainmentLeisure":
              endpointCategory = "entertainment-leisure";
              break;
            default:
              continue;
          }

          const detailUrl = buildApiUrl(
            `${endpointCategory}/detail/${item.id}?language=it`
          );

          // Execute the fetch "empty". The Service Worker intercepts it and saves the JSON.
          await fetch(detailUrl, {
            method: "GET",
            credentials: "include",
          });
        } catch (e) {
          // Silent error, we do not want to disturb the user
        }
      }
      console.log("Prefetch: Completato!");
    };

    // Delay of 3 seconds to avoid slowing down the initial page load
    const timer = setTimeout(() => {
      prefetchDetails();
    }, 3000);

    return () => clearTimeout(timer);
  }, [elements, loadingElements]);

  /**
   * Filter elements based on selected category and badge
   * AND deduplicate if no category is selected
   */
  const filteredElements = useMemo(() => {
    //Apply standard filters
    const matches = elements.filter((element) => {
      const categoryMatch =
        selectedCategory === null || element.category === selectedCategory;
      const badgeMatch =
        selectedBadge === null || element.badge === selectedBadge;
      return categoryMatch && badgeMatch;
    });

    //If a specific categories is selected, we return all
    if (selectedCategory !== null) {
      return matches;
    }

    // 3. Se siamo in visualizzazione "All" (selectedCategory === null),
    // dobbiamo rimuovere i duplicati basandoci sull'ID.
    const uniqueIds = new Set();
    const distinctElements: ElementItem[] = [];

    for (const item of matches) {
      if (!uniqueIds.has(item.id)) {
        uniqueIds.add(item.id);
        distinctElements.push(item);
      }
    }

    return distinctElements;
  }, [elements, selectedCategory, selectedBadge]);

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
            logout={handleLogout}
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
                No items available for the selected filters.
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
