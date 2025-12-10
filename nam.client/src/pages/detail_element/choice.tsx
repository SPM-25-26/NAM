import { Stack } from "@mui/material";
import MyAppBar from "../../components/appbar";
import { CategoryApi } from "./hooks/IDetailElement";
import Button from "../../components/button";
import { useNavigate } from "react-router-dom";

const ChoicePageTest: React.FC = () => {
  const navigate = useNavigate();
  return (
    <div>
      <MyAppBar title={"Choice Test"} back />
      <Stack direction="column" alignItems="center" spacing={1}>
        <Button
          label={CategoryApi.ART_CULTURE}
          action={() =>
            navigate("/detail-element", {
              state: {
                id: "9367162f-d261-4204-b1c4-6813be00e12c",
                category: CategoryApi.ART_CULTURE,
              },
            })
          }
        />
        <Button
          label={CategoryApi.ARTICLE}
          action={() =>
            navigate("/detail-element", {
              state: {
                id: "5136cc0c-0c5d-4d40-ae0d-925919ed51d1",
                category: CategoryApi.ARTICLE,
              },
            })
          }
        />
        <Button
          label={CategoryApi.EAT_AND_DRINK}
          action={() =>
            navigate("/detail-element", {
              state: {
                id: "140a8d04-fc1a-4839-a2eb-71e31e2229e8",
                category: CategoryApi.EAT_AND_DRINK,
              },
            })
          }
        />
        <Button
          label={CategoryApi.ENTERTAINMENT_LEISURE}
          action={() =>
            navigate("/detail-element", {
              state: {
                id: "dde599c4-8b82-41d1-aabf-1eca7d5df4f4",
                category: CategoryApi.ENTERTAINMENT_LEISURE,
              },
            })
          }
        />
        <Button
          label={CategoryApi.EVENTS}
          action={() =>
            navigate("/detail-element", {
              state: {
                id: "4b13e510-3af3-41cb-95c6-8b39b4090969",
                category: CategoryApi.EVENTS,
              },
            })
          }
        />
        <Button
          label={CategoryApi.NATURE}
          action={() =>
            navigate("/detail-element", {
              state: {
                id: "43166480-e75c-4171-bf3f-fcb356c7ab48",
                category: CategoryApi.NATURE,
              },
            })
          }
        />
        <Button
          label={CategoryApi.ORGANIZATION}
          action={() =>
            navigate("/detail-element", {
              state: {
                id: "02575020447",
                category: CategoryApi.ORGANIZATION,
              },
            })
          }
        />
        <Button
          label={CategoryApi.ROUTES}
          action={() =>
            navigate("/detail-element", {
              state: {
                id: "395dcfe9-59ac-434e-a25e-dbc25ce5431a",
                category: CategoryApi.ROUTES,
              },
            })
          }
        />
        <Button
          label={CategoryApi.SHOPPING}
          action={() =>
            navigate("/detail-element", {
              state: {
                id: "0105daa6-3cd4-43ab-8493-5d27368447fb",
                category: CategoryApi.SHOPPING,
              },
            })
          }
        />
        <Button
          label={CategoryApi.SLEEP}
          action={() =>
            navigate("/detail-element", {
              state: {
                id: "07b8c98b-d9ef-4e22-8f16-2260f64664bb",
                category: CategoryApi.SLEEP,
              },
            })
          }
        />
      </Stack>
    </div>
  );
};

export default ChoicePageTest;
