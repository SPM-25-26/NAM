import { Box, Stack } from "@mui/material";
import { CustomSelectionCard } from "../components/customButtonRadio";

interface StepYesOrNoImageProps {
    onSelect: (id: string) => void;
    onBack?: () => void;
    urlImage?: string;

    // Add explicit IDs to separate text (label) from value (id)
    id_1?: string;
    label_1?: string;
    description_1?: string;

    id_2?: string;
    label_2?: string;
    description_2?: string;

    selectedValue?: string | null;
}

export function StepYesOrNoImage({
    onSelect,
    urlImage,

    // Default ID uguali alle Label se non passati (retrocompatibilità)
    label_1 = "Yes",
    id_1,
    description_1 = "",

    label_2 = "NO",
    id_2,
    description_2 = "",

    selectedValue, // Receive the current value directly from props
}: StepYesOrNoImageProps) {

    // CALCULATE EFFECTIVE IDS
    // If you don't pass id_1, use the label as a fallback (as it did before)
    const value1 = id_1 || label_1;
    const value2 = id_2 || label_2;

    // REMOVED INTERNAL useState
    // The component is now "pure": it shows what 'selectedValue' tells it

    const handleSelection = (value: string) => {
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
                    {/* OPZIONE 1 */}
                    <CustomSelectionCard
                        label={label_1}
                        description={description_1}
                        value={value1} // Here we pass the ID, not the label

                        // Compare the ID of this button with the one selected in the DB
                        selectedValue={selectedValue || ""}

                        onChange={() => handleSelection(value1)}
                    />

                    {/* OPZIONE 2 */}
                    <CustomSelectionCard
                        label={label_2}
                        description={description_2}
                        value={value2} // Here we pass the ID, not the label

                        // Compare the ID of this button with the one selected in the DB
                        selectedValue={selectedValue || ""}

                        onChange={() => handleSelection(value2)}
                    />
                </Stack>
            </Stack>
        </Box>
    );
}