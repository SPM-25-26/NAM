import type { useUserSurvey } from "./hooks/useUserSurvey";
import type { StepConfig } from "../../components/stepper/stepConfig";
import { StepGrid } from "../../components/stepper/step/gridStep";
import { StepList } from "../../components/stepper/step/listStep";
import { StepChipSelection } from "../../components/stepper/step/chipStep";
import { StepYesOrNoImage } from "../../components/stepper/step/yesOrnNo";
import imageArtAndCulture from "../../assets/categories/artAndCulture.jpg";
import imageArticles from "../../assets/categories/articles.jpg";
import imageEvents from "../../assets/categories/events.jpg";
import imageLeisures from "../../assets/categories/leisure.jpg";
import imageOrganizations from "../../assets/categories/organizations.jpg";
import imageNature from "../../assets/categories/nature.jpg";

type SurveyState = ReturnType<typeof useUserSurvey>;

export const surveyIntroConfig = {
  title: "We’re building your experience",
  description:
    "Answer a few questions to help us personalize content, features, and recommendations based on your preferences.",
  buttonLabel: "Start Survey",
  headerTitle: "",
  imageSrc: "ImageSurveyIntro",
  infoItems: [
    { iconName: "AccessTime", text: "Takes less than 1 minute" },
    {
      iconName: "LockOutlined",
      text: "Used only to personalize your experience",
    },
  ],
};
export const surveyCompleteConfig = {
  title: "All set! We’re tailoring your experience",
  subtitle:
    "Give us a moment while we process your answers and prepare your personalized dashboard.",
  buttonLabel: "Go to Home",
};

export const surveySteps = (state: SurveyState): StepConfig<SurveyState>[] => [
  {
    label: "Interessi Principali",
    description: "Cosa ti appassiona di più?",
    render: () => (
      <StepGrid
        onBack={state.back}
        selectedValue={state.data.interest}
        onSelect={(newIds) => {
          state.updateData(
            "interest",
            updateCategorySelection(state.data.interest, newIds)
          );
        }}
        options={[
          {
            id: "article",
            label: "Articoli",
            description: "Letture e news",
            image: imageArticles,
          },
          {
            id: "art_culture",
            label: "Arte e Cultura",
            description: "Musei e storia",
            image: imageArtAndCulture,
          },
          {
            id: "events",
            label: "Eventi",
            description: "Spettacoli e incontri",
            image: imageEvents,
          },
          {
            id: "organization",
            label: "Organizzazioni",
            description: "Enti e no-profit",
            image: imageOrganizations,
          },
          {
            id: "nature",
            label: "Natura",
            description: "Outdoor e fauna",
            image: imageNature,
          },
          {
            id: "leisure",
            label: "Tempo Libero",
            description: "Divertimento",
            image: imageLeisures,
          },
        ]}
      />
    ),
  },

  {
    label: "Stile di Viaggio",
    description: "Aiutaci a capire come ami vivere il territorio",
    render: () => (
      <StepList
        selectedValue={state.data.travelStyle}
        onSelect={(id) => {
          state.updateData(
            "travelStyle",
            updateCategorySelection(state.data.travelStyle, id)
          );
        }}
        options={[
          {
            id: "instancabile",
            label: "L'Instancabile",
            description: "Trekking e lunghe camminate",
          },
          {
            id: "culturale",
            label: "Il Culturale",
            description: "Storia, borghi e monumenti",
          },
          {
            id: "buongustaio",
            label: "Il Buongustaio",
            description: "Enogastronomia e sagre",
          },
          {
            id: "relax",
            label: "Il Relax-Seeker",
            description: "Viste panoramiche e ritmi lenti",
          },
          {
            id: "social",
            label: "Il Social-Traveler",
            description: "Posti instagrammabili ed eventi",
          },
        ]}
      />
    ),
  },

  {
    label: "Compagni di viaggio",
    description: "Personalizzeremo i suggerimenti per il tuo gruppo",
    render: () => (
      <StepChipSelection
        selectedValue={state.data.travelCompanions}
        options={[
          { id: "solo", label: "Da solo/a" },
          { id: "couple", label: "In coppia" },
          { id: "family", label: "Famiglia (bimbi piccoli)" },
          { id: "friends", label: "Gruppo di amici" },
          { id: "pets", label: "Con animali 🐾" },
        ]}
        onSelect={(id) => {
          state.updateData(
            "travelCompanions",
            updateCategorySelection(state.data.travelCompanions, id)
          );
        }}
      />
    ),
  },

  {
    label: "Modalità Scoperta",
    description:
      "Preferisci restare sui tuoi interessi abituali o esplorare nuove possibilità?",
    render: () => (
      <StepYesOrNoImage
        label_1="Sorprendimi"
        description_1="Mostrami luoghi nuovi e categorie che non ho ancora esplorato."
        description_2="Proponimi attività basate esclusivamente sui miei interessi scelti."
        label_2="Rassicurami"
        urlImage="https://images.unsplash.com/photo-1506744038136-46273834b3fb"
        initialValue={state.data.discoveryMode}
        onSelect={(id) => {
          state.updateData("discoveryMode", id);
          state.next();
        }}
      />
    ),
  },

  {
    label: "Raggio d'azione",
    description: "Quanto sei disposto a spostarti dalla tua posizione attuale?",
    render: () => (
      <StepYesOrNoImage
        label_1="Km Zero"
        description_1="Mostrami solo quello che posso raggiungere in pochi minuti (entro 10km)."
        label_2="Esploratore"
        description_2="Fammi scoprire tesori e borghi in tutta la regione (oltre 50km)."
        initialValue={state.data.travelRange}
        onSelect={(id) => {
          state.updateData("travelRange", id);
          state.next();
        }}
      />
    ),
  },

  {
    label: "Quanti anni hai?",
    description:
      "Ci aiuta a filtrare attività e suggerirti eventuali sconti o agevolazioni nei musei.",
    render: () => (
      <StepList
        selectedValue={state.data.ageRange ? [state.data.ageRange] : []}
        onSelect={(id) => {
          state.updateData("ageRange", id);
        }}
        options={[
          {
            id: "under_25",
            label: "Under 25",
            description: "Sconti studenti e contenuti dinamici",
          },
          {
            id: "25_50",
            label: "25 - 50 anni",
            description: "Itinerari ed esperienze nel territorio",
          },
          {
            id: "over_50",
            label: "Over 50",
            description: "Comfort, cultura e ritmi più lenti",
          },
          {
            id: "not_defined",
            label: "Preferisco non dirlo",
            description: "Riceverai suggerimenti generici",
          },
        ]}
      />
    ),
  },
];

function updateCategorySelection(
  category: string[] = [],
  newIds: string
): string[] {
  const set = new Set(category);
  if (set.has(newIds)) {
    set.delete(newIds);
  } else {
    set.add(newIds);
  }
  return Array.from(set);
}
