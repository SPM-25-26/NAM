export interface Organizer {
  taxCode: string;
  legalName: string;
  website: string;
}

export interface TicketAndCost {
  description: string | undefined;
  priceSpecificationCurrencyValue: number | undefined;
  currency: string | undefined;
  validityDescription: string | undefined;
  validityStartDate: string | undefined;
  validityEndDate: string | undefined;
  userTypeName: string | undefined;
  userTypeDescription: string | undefined;
  ticketDescription: string | undefined;
}
export interface CreativeWork {
  type: string;
  url: string;
}

export interface Service {
  name?: string;
  description?: string;
  identifier?: string;
  imagePath?: string;
}
export interface Neighbor {
    id: string;
    featureCard: {
        entityId: string;
        title: string;
        category: string;
        imagePath: string;
        extraInfo: string;
    };
}

export interface Stage {
    id: string;
    stageMobile: {
        category: string;
        poiIdentifier: string;
        poiOfficialName: string;
        poiImagePath: string;
        poiAddress: string;
    };
}
export interface Element {
  identifier?: string;
  script?: string;
  updatedAt?: string;
  title?: string;
  address?: string;
  description?: string;
  typology?: string;
  primaryImage?: string;
  gallery?: string[];
  audience?: string;
  email?: string;
  telephone?: string;
  website?: string;
  facebook?: string;
  instagram?: string;
  organizationWebsite?: string;
  organizationEmail?: string;
  organizationFacebook?: string;
  organizationInstagram?: string;
  organizationTelephone?: string;
  pathTheme?: string,
  travellingMethod?: string;
  securityLevel?: string;
  duration?: string;
  latitude?: number;
  longitude?: number;
  date?: string;
  officialName?: string;
  creativeWorks?: CreativeWork[];
  neighbors: Neighbor[];
  stages: Stage[];
  startDate?: string;
  endDate?: string;
  services?: Service[];
  organizer?: Organizer;
  ticketsAndCosts?: TicketAndCost[];
  subtitle?: string;
  timeToRead?: string;
  type?: string;
  paragraphs?: IParagraph[];
  site?: WebSiteInfo;
  offers?: Offer[];
}
export interface Offer {
  description: string;
  priceSpecificationCurrencyValue: number;
  currency: string;
  validityDescription: string;
  validityStartDate: Date;
  validityEndDate: Date;
  userTypeName: string;
  userTypeDescription: string;
  ticketDescription: string;
}

export interface WebSiteInfo {
  identifier: string;
  officialName: string;
  imagePath: string;
  category: string;
}
/**
 * Represents a paragraph within a story, including its main content, position, and optional metadata.
 *
 * @property title - Title or header of the paragraph.
 * @property script - Main textual content of the paragraph.
 * @property position - Ordinal position of the paragraph within the story (0, 1, 2...).
 * @property subtitle - Subtitle of the paragraph, if present.
 * @property region - Geographical or thematic area related to the paragraph.
 *
 * Reference Data (Related POI):
 * @property referenceIdentifier - Unique ID of the mentioned point of interest (POI).
 * @property referenceName - Official name of the point of interest.
 * @property referenceCategory - Category of the point of interest (e.g., 'Museum', 'Park').
 * @property referenceImagePath - Path to the image associated with the reference.
 * @property referenceLongitude - Longitude of the point of interest.
 * @property referenceLatitude - Latitude of the point of interest.
 */
export interface IParagraph {
  title?: string;
  script: string;
  position: number;
  subtitle?: string;
  region?: string;
  referenceIdentifier?: string;
  referenceName?: string;
  referenceCategory?: string;
  referenceImagePath?: string;
  referenceLongitude?: number;
  referenceLatitude?: number;
}

const CategoryApi = {
  ART_CULTURE: "art-culture",
  ARTICLE: "article",
  EAT_AND_DRINK: "eat-and-drink",
  ENTERTAINMENT_LEISURE: "entertainment-leisure",
  EVENTS: "public-event",
  NATURE: "nature",
  ROUTE: "routes",
  ORGANIZATION: "organizations",
  SERVICE: "services",
  SHOPPING: "shopping",
  SLEEP: "sleep",
  TYPICAL_PRODUCTS: "typical-products",
} as const;
type CategoryApi = (typeof CategoryApi)[keyof typeof CategoryApi];

export { CategoryApi };

/* Used for converting category and enabling automatic navigation to the detail page */
const InputKeyToEnumName: { [key: string]: keyof typeof CategoryApi } = {
  ArtCulture: "ART_CULTURE",
  Article: "ARTICLE",
  EatAndDrink: "EAT_AND_DRINK",
  EntertainmentLeisure: "ENTERTAINMENT_LEISURE",
  Events: "EVENTS",
  Nature: "NATURE",
  Route: "ROUTE",
  Organization: "ORGANIZATION",
  Service: "SERVICE",
  Shopping: "SHOPPING",
  Sleep: "SLEEP",
  TypicalProducts: "TYPICAL_PRODUCTS",
};

/**
 * Converts an API string (e.g., "ArtCulture") to the corresponding CategoryApi value (e.g., "art-culture").
 * @param a The API string (case-sensitive, e.g., "ArtCulture").
 * @returns A value of type CategoryApi (string in kebab-case).
 */
export function stringToCategoryAPI(a: string): CategoryApi {
  // 1. Get the enum key name (e.g., "ART_CULTURE") from the input string
  const enumKey = InputKeyToEnumName[a];

  // 2. Check if the mapping was successful
  if (enumKey && enumKey in CategoryApi) {
    // 3. If the key exists and belongs to the enum, return its value (e.g., "art-culture")
    return CategoryApi[enumKey];
  }

  // 4. If the string is not mapped, return the default
  console.warn(
    `Category string not mapped or invalid: "${a}". Using default: ${CategoryApi.SLEEP}`
  );
  return CategoryApi.SLEEP;
}
