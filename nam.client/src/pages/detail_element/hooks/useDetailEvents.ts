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
                const query = "?language=it";
                // Service Worker will intercept this request.
                // If offline and cached, it returns the JSON. If not, it throws, catching below.
                const res = await fetch(
                    buildApiUrl(
                        `${category.toString()}${pathDetails}${identifier}${query}`
                    ),
                    {
                        method: "GET",
                        headers: {
                            Accept: "application/json",
                        },
                        credentials: "include",
                    }
                );
                if (res.ok) {
                    const json = await res.json();
                    const safeData = structuredClone(json);
                    if (isMounted) setData(safeData);
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
            "id": "3d6f1f0a-88fa-4003-be17-013ee15ca9af",
            "featureCard": {
                "entityId": "ad065550-f374-43ac-b0cc-6d0af22941b0",
                "title": "Terme Romane",
                "category": "ArtCulture",
                "imagePath": "/Media/POI/primary-thumb-ad065550-f374-43ac-b0cc-6d0af22941b0.webp",
                "extraInfo": "199 m"
            }
        },
        {
            "id": "9457e044-9f27-4f32-8f95-0382c037a8b1",
            "featureCard": {
                "entityId": "cb25f661-7d70-47b2-a491-997345901512",
                "title": "Sotterranei e Domus di Palazzo Ottoni",
                "category": "ArtCulture",
                "imagePath": "/Media/POI/primary-thumb-cb25f661-7d70-47b2-a491-997345901512.webp",
                "extraInfo": "1,1 km"
            }
        },
        {
            "id": "5be75a71-c92a-4081-b8f4-0613f35f1fc2",
            "featureCard": {
                "entityId": "ced61939-d2a7-4601-b81e-77b86b6c4b6f",
                "title": "Chiesa di San Filippo Neri",
                "category": "ArtCulture",
                "imagePath": "/Media/POI/primary-thumb-ced61939-d2a7-4601-b81e-77b86b6c4b6f.webp",
                "extraInfo": "134 m"
            }
        },
        {
            "id": "98d2c8c7-8d7e-4e5e-9ec3-07018a50c524",
            "featureCard": {
                "entityId": "cb25f661-7d70-47b2-a491-997345901512",
                "title": "Sotterranei e Domus di Palazzo Ottoni",
                "category": "ArtCulture",
                "imagePath": "/Media/POI/primary-thumb-cb25f661-7d70-47b2-a491-997345901512.webp",
                "extraInfo": "1,1 km"
            }
        },
        {
            "id": "3f02424f-9701-485d-bf28-0acb4849e699",
            "featureCard": {
                "entityId": "ced61939-d2a7-4601-b81e-77b86b6c4b6f",
                "title": "Chiesa di San Filippo Neri",
                "category": "ArtCulture",
                "imagePath": "/Media/POI/primary-thumb-ced61939-d2a7-4601-b81e-77b86b6c4b6f.webp",
                "extraInfo": "134 m"
            }
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