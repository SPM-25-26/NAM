import { useState, useEffect, useCallback } from "react";
import { buildApiUrl } from "../../../config";
import { useNavigate } from "react-router-dom";

export interface UserProfile {
  email: string;
  fullName?: string;
  avatarUrl?: string;
  memberSince?: string;
  preferencesSet: boolean;
}

export function useProfile() {
  const [isOffline, setIsOffline] = useState(!navigator.onLine);
  const [user, setUser] = useState<UserProfile | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();
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

  const fetchProfile = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const response = await fetch(buildApiUrl("/user/profile"), {
        method: "GET",
        credentials: "include",
        headers: {
          Accept: "application/json",
        },
      });

      if (!response.ok) {
        throw new Error(`Errore server: ${response.status}`);
      }

      const data = (await response.json()) as UserProfile;
      setUser(data);
    } catch (err) {
      console.error("Errore fetch profilo:", err);
      setError("Impossibile caricare i dati del profilo.");
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchProfile();
  }, [fetchProfile]);

  const authLogout = async () => {
    setLoading(true);
    setError(null);
    try {
      const response = await fetch(buildApiUrl("auth/logout"), {
        method: "POST",
        credentials: "include",
      });

      if (!response.ok) {
        console.error("Logout failed");
      } else {
        setUser(null);
        navigate("/login");
      }
    } catch (err) {
      console.error("Logout error:", err);
    } finally {
      setLoading(false);
    }
  };

  return {
    state: {
      user,
      loading,
      error,
      isOffline,
    },
    actions: {
      logout: authLogout,
      refresh: fetchProfile,
    },
  };
}
