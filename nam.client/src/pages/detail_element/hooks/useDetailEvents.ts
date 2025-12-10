import { useEffect, useState } from "react";
import type { CategoryApi, Element } from "./IDetailElement";
//TODO: replace with internal url
const BASE_URL = "https://apispm.eppoi.io/api/";
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
        const pathDetails = "/detail/";
        const query = "?language=en";
        const res = await fetch(
          BASE_URL + category.toString() + pathDetails + identifier + query,
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
