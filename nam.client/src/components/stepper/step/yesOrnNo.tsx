import { Box, Stack } from "@mui/material";
import { useState } from "react";
import { CustomSelectionCard } from "../components/customButtonRadio";

interface StepYesOrNoImageProps {
  onSelect: (id: string) => void;
  onBack?: () => void;
  urlImage?: string;
  label_1?: string;
  description_1?: string;
  label_2?: string;
  description_2?: string;
  initialValue?: string;
}

export function StepYesOrNoImage({
  onSelect,
  urlImage,
  label_1 = "Yes",
  description_1 = "",
  label_2 = "NO",
  description_2 = "",
  initialValue,
}: StepYesOrNoImageProps) {
  const [selectedValue, setSelectedValue] = useState<string>(
    initialValue || ""
  );

  const handleSelection = (value: string) => {
    setSelectedValue(value);
    onSelect(value);
  };

  return (
    <Box
      sx={{
        width: "100%",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        minHeight: "100%",
      }}
    >
      <Stack
        direction="column"
        spacing={{ xs: 2, md: 3 }}
        alignItems="center"
        sx={{
          width: "100%",
          px: 2,
        }}
      >
        {urlImage && (
          <Box
            component="img"
            src={urlImage}
            alt="Step visual"
            sx={{
              width: "100%",
              objectFit: "cover",
              borderRadius: 4,
            }}
          />
        )}

        <Stack
          direction={{ xs: "column", sm: "row" }}
          justifyContent="center"
          alignItems="stretch"
          spacing={2}
          sx={{ width: "100%" }}
        >
          <CustomSelectionCard
            label={label_1}
            description={description_1}
            value={label_1}
            selectedValue={selectedValue}
            onChange={() => handleSelection(label_1)}
          />
          <CustomSelectionCard
            label={label_2}
            description={description_2}
            value={label_2}
            selectedValue={selectedValue}
            onChange={() => handleSelection(label_2)}
          />
        </Stack>
      </Stack>
    </Box>
  );
}
