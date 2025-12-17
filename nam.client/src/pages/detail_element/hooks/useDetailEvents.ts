import { useEffect, useState } from "react";
import type { CategoryApi, Element } from "./IDetailElement";
import { buildApiUrl } from "../../../config";
//TODO: replace with internal url

export function useLocationEvent(identifier: string, category: CategoryApi) {
  const [data, setData] = useState<Element | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    let isMounted = true;

    async function fetchProd() {
      setLoading(true);
      setError(null);

      try {
        //-----------------//-----------------//-----------------
        //TODO: REMOVE
        if (identifier == "mock_test") return setData(mockElement);
        //-----------------//-----------------//-----------------
        const pathDetails = "/detail/";
        const query = "?language=en";
        const res = await fetch(
          buildApiUrl(
            `${category.toString()}${pathDetails}${identifier}${query}`
          ),
          {
            method: "GET",
            headers: {
              Accept: "application/json",
            },
          }
        );
        if (res.ok) {
          const json = await res.json();
          if (isMounted) setData(json);
        } else throw new Error(`Errore API: ${res.status}`);
      } catch (err) {
        if (isMounted) setError((err as Error).message);
      } finally {
        if (isMounted) setLoading(false);
      }
    }

    fetchProd();

    return () => {
      isMounted = false;
    };
  }, [identifier, category]);

  return { data, loading, error };
}

const mockElement: Element = {
  identifier: "evt-001",
  title: "Digital Art Exhibition 2025",
  subtitle: "Exploring creativity through technology",
  description:
    "An immersive exhibition showcasing contemporary digital artists.",
  typology: "Exhibition",
  address: "Via Roma 10, Milan, Italy",
  primaryImage: "/images/events/digital-art/main.jpg",
  gallery: [
    "Media/POI/primary-e71c75ee-77c2-40c3-9add-a26dc7ed19c8.webp",
    "Media/POI/primary-e71c75ee-77c2-40c3-9add-a26dc7ed19c8.webp",
  ],
  audience: "General public",
  email: "info@digitalartexpo.com",
  website: "https://www.digitalartexpo.com",
  latitude: 45.4642,
  longitude: 9.19,
  startDate: "2025-03-10",
  endDate: "2025-06-30",
  timeToRead: "5 min",
  type: "Event",

  organizer: {
    taxCode: "IT12345678901",
    legalName: "Digital Art Association",
    website: "https://www.digitalartassociation.org",
  },

  neighbors: [
    {
      entityId: "poi-101",
      title: "Modern Art Museum",
      category: "Museum",
      imagePath: "Media/POI/primary-e71c75ee-77c2-40c3-9add-a26dc7ed19c8.webp",
      extraInfo: "5 minutes walk",
    },
    {
      entityId: "poi-102",
      title: "City Park",
      category: "Park",
      imagePath: "Media/POI/primary-e71c75ee-77c2-40c3-9add-a26dc7ed19c8.webp",
      extraInfo: "Outdoor relaxation area nearby",
    },
  ],

  services: [
    {
      name: "Guided Tour",
      description: "Daily guided tours with expert curators",
      identifier: "service-guided-tour",
      imagePath: "Media/POI/primary-e71c75ee-77c2-40c3-9add-a26dc7ed19c8.webp",
    },
    {
      name: "Audio Guide",
      description: "Multilingual audio guide available",
      identifier: "service-audio-guide",
    },
  ],

  creativeWorks: [
    {
      type: "Video",
      url: "https://www.youtube.com/watch?v=example",
    },
    {
      type: "Article",
      url: "https://blog.digitalartexpo.com/behind-the-scenes",
    },
  ],

  ticketsAndCosts: [
    {
      description: "Standard ticket",
      priceSpecificationCurrencyValue: 15,
      currency: "EUR",
      validityDescription: "Valid for one day",
      validityStartDate: "2025-03-10",
      validityEndDate: "2025-06-30",
      userTypeName: "Adult",
      userTypeDescription: "Adults 18+",
      ticketDescription: "Full access to exhibition",
    },
    {
      description: "Reduced ticket",
      priceSpecificationCurrencyValue: 8,
      currency: "EUR",
      validityDescription: "Valid for one day",
      validityStartDate: "2025-03-10",
      validityEndDate: "2025-06-30",
      userTypeName: "Student",
      userTypeDescription: "Students with valid ID",
      ticketDescription: "Discounted access",
    },
  ],

  offers: [
    {
      description: "Early bird ticket",
      priceSpecificationCurrencyValue: 10,
      currency: "EUR",
      validityDescription: "Available until end of March",
      validityStartDate: new Date("2025-03-01"),
      validityEndDate: new Date("2025-03-31"),
      userTypeName: "Adult",
      userTypeDescription: "Limited early access offer",
      ticketDescription: "Early bird full access",
    },
  ],

  site: {
    identifier: "site-001",
    officialName: "Digital Art Expo Venue",
    imagePath: "Media/POI/primary-e71c75ee-77c2-40c3-9add-a26dc7ed19c8.webp",
    category: "Exhibition Center",
  },

  paragraphs: [
    {
      position: 0,
      title: "Introduction",
      script:
        "Digital Art Exhibition 2025 brings together artists who use technology as their primary medium.",
    },
    {
      position: 1,
      title: "Featured Artists",
      script:
        "The exhibition features international artists working with AI, VR, and generative art.",
      referenceIdentifier: "poi-101",
      referenceName: "Modern Art Museum",
      referenceCategory: "Museum",
      referenceImagePath:
        "Media/POI/primary-e71c75ee-77c2-40c3-9add-a26dc7ed19c8.webp",
      referenceLatitude: 45.4645,
      referenceLongitude: 9.1898,
    },
  ],
};
