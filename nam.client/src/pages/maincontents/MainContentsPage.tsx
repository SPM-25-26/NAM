import React, { useEffect, useMemo, useState } from "react";
import FlightIcon from "@mui/icons-material/Flight";
import {
  Box,
  Card,
  Container,
  Typography,
  useTheme,
  CircularProgress,
} from "@mui/material";
import { buildApiUrl } from "../../config";
import ElementCard from "../../components/ElementCardComponent";
import CategorySelect from "../../components/SelectComponent";
import type { CategoryOption } from "../../components/SelectComponent";
import { useNavigate } from "react-router-dom";
import { stringToCategoryAPI } from "../detail_element/hooks/IDetailElement";
import MyAppBar from "../../components/appbar";

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
   * Verify user authentication on component mount
   */
  useEffect(() => {
    const checkAuth = async () => {
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
   * Fetches the actual image blob from /image/external and returns a local object URL
   */
  const fetchImageBlob = async (
    imagePath: string
  ): Promise<string | undefined> => {
    if (!imagePath) return undefined;

    // Clean the path first as requested previously
    const cleanedPath = imagePath
      .replace(/-thumb-/g, "-")
      .replace(/-thumb(?=\.[^.]+$)/, "");

    try {
      const response = await fetch(
        buildApiUrl("image/external?imagePath=" + cleanedPath),
        {
          method: "GET",
          headers: {
            Accept: "application/json",
          },
          credentials: "include",
        }
      );

      if (response.ok) {
        const blob = await response.blob();
        return URL.createObjectURL(blob);
      }
    } catch (error) {
      console.error("Failed to fetch image:", error);
    }
    return undefined;
  };

  /**
   * Fetches card list data from a specific category endpoint
   */
  const fetchCategoryData = async (
    endpoint: string,
    category: string
  ): Promise<ElementItem[]> => {
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

    // Map data and fetch images in parallel for this batch
    // Note: This might be heavy if there are many items.
    const itemsWithImages = await Promise.all(
      data.map(async (item: ApiCardItem, index: number) => {
        let imageUrl: string | undefined = undefined;

        if (item.imagePath) {
          imageUrl = await fetchImageBlob(item.imagePath);
        }

        return {
          id:
            item.entityId?.toString() ?? item.taxCode ?? `${category}-${index}`,
          title: item.entityName || "Untitled",
          badge: item.badgeText || "",
          address: item.address || "",
          imageUrl: imageUrl,
          date: item.date,
          category: category,
        };
      })
    );

    return itemsWithImages;
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

    // Cleanup function to revoke object URLs when component unmounts or elements update
    return () => {
      elements.forEach((el) => {
        if (el.imageUrl && el.imageUrl.startsWith("blob:")) {
          URL.revokeObjectURL(el.imageUrl);
        }
      });
    };
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

  /**
   * Filter elements based on selected category and badge
   */
  const filteredElements = useMemo(() => {
    return elements.filter((element) => {
      const categoryMatch =
        selectedCategory === null || element.category === selectedCategory;
      const badgeMatch =
        selectedBadge === null || element.badge === selectedBadge;
      return categoryMatch && badgeMatch;
    });
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
          <MyAppBar
            title={"Eppoi"}
            logout={handleLogout}
            back
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
                  <Box key={item.id} sx={{ height: "100%" }}>
                    <ElementCard
                      title={item.title}
                      badge={item.badge}
                      address={item.address}
                      imageUrl={item.imageUrl}
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
    </Box>
  );
};

export default MainContentsPage;
