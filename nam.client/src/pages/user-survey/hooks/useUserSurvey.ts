import { useState } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import { buildApiUrl } from "../../../config";

export type SurveyData = {
  interest?: string[];
  travelStyle?: string[];
  ageRange?: string;
  travelRange?: string;
  travelCompanions?: string[];
  discoveryMode?: string;
};

export const useUserSurvey = () => {
  const navigate = useNavigate();
  const location = useLocation();

  // Recuperiamo i dati passati dalla pagina profilo (se esistono)
  const previousData = location.state?.previousData;

  const [activeStep, setActiveStep] = useState(0);
  // Se abbiamo dati precedenti (stiamo modificando), NON mostriamo l'intro
  const [initSurvey, setInitSurvey] = useState(!previousData);


    // Inizializziamo lo stato con i dati precedenti (o valori di default vuoti)
    const [data, setData] = useState<SurveyData>({
        interest: previousData?.interest ?? [],
        travelStyle: previousData?.travelStyle ?? [],
        ageRange: previousData?.ageRange ?? undefined,
        travelRange: previousData?.travelRange ?? undefined,
        travelCompanions: previousData?.travelCompanions ?? [],
        discoveryMode: previousData?.discoveryMode ?? undefined,
    });

  const updateData = <K extends keyof SurveyData>(
    key: K,
    value: SurveyData[K]
  ) => setData((prev) => ({ ...prev, [key]: value }));
  const next = () => setActiveStep((s) => s + 1);
  const back = () => setActiveStep((s) => Math.max(s - 1, 0));

  return {
    activeStep,
    initSurvey,
    data,
    updateData,
    start: () => setInitSurvey(false),
    next,
    skip: () => navigate(-1),
    back,
    submit: async () => {
        const payload = {
            interest: data.interest || [],            
            travelStyle: data.travelStyle || [],      
            ageRange: data.ageRange || null,          
            travelRange: data.travelRange || null,    
            travelCompanions: data.travelCompanions || [],
            discoveryMode: data.discoveryMode || null
      };

      try {
          const response = await fetch(buildApiUrl("user/update-questionaire"), {
          method: "POST",
          credentials: "include",
          headers: {
            "Content-Type": "application/json",
          },
          // stringify body
          body: JSON.stringify(payload, null, 2),
        });
        if (!response.ok) {
          throw new Error(`Server error: ${response.statusText}`);
        }
      } catch (error) {
        console.error("Failed to submit survey:", error);
      }
    },
  };
};
