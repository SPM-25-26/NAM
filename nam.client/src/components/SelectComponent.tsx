import React, { useRef } from "react";
import { Box, useTheme, Chip, Stack } from "@mui/material";
import ChevronRightIcon from "@mui/icons-material/ChevronRight";
import ChevronLeftIcon from "@mui/icons-material/ChevronLeft";

export type CategoryOption = {
    value: string | null;
    label: string;
};

interface SelectComponentProps {
    label: string;
    value: string | null;
    options: CategoryOption[];
    onChange: (value: string | null) => void;
    accentColor?: string; // Ora accetta anche gradienti es: "linear-gradient(...)"
}

const SelectComponent: React.FC<SelectComponentProps> = ({
    value,
    options,
    onChange,
    accentColor,
}) => {
    const theme = useTheme();
    // Default fallback se non c'è accentColor
    const accent = accentColor ?? theme.palette.primary.main;

    const scrollRef = useRef<HTMLDivElement | null>(null);

    const handleScroll = (direction: "left" | "right") => {
        const node = scrollRef.current;
        if (!node) return;
        const scrollAmount = 150;
        const newScrollLeft =
            direction === "right"
                ? node.scrollLeft + scrollAmount
                : node.scrollLeft - scrollAmount;

        node.scrollTo({
            left: newScrollLeft,
            behavior: "smooth",
        });
    };

    return (
        <Box
            sx={{
                backgroundColor: theme.palette.background.paper,
                borderRadius: theme.shape.borderRadius,
                padding: 1.5,

                // --- MODIFICA PER BORDO GRADIENTE ---
                // 1. Rendiamo il bordo reale trasparente
                border: "3px solid transparent",
                // 2. Usiamo due background: 
                //    - Il primo è il colore di sfondo (paper) confinato al padding-box (contenuto)
                //    - Il secondo è l'accento (gradiente o colore) confinato al border-box
                background: `
                    linear-gradient(${theme.palette.background.paper}, ${theme.palette.background.paper}) padding-box,
                    ${accent} border-box
                `,
                // ------------------------------------
            }}
        >
            <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
                <Box sx={{ display: "flex", alignItems: "center" }}>
                    <ChevronLeftIcon
                        fontSize="small"
                        sx={{ cursor: "pointer" }}
                        onClick={() => handleScroll("left")}
                    />
                </Box>

                <Box
                    ref={scrollRef}
                    sx={{
                        flex: 1,
                        overflowX: "auto",
                        overflowY: "hidden",
                        "&::-webkit-scrollbar": { display: "none" },
                        msOverflowStyle: "none",
                        scrollbarWidth: "none",
                    }}
                >
                    <Stack
                        direction="row"
                        spacing={1}
                        sx={{ py: 0.5, width: "max-content" }}
                    >
                        {options.map((opt) => {
                            const isSelected =
                                (value === null && opt.value === null) ||
                                (value !== null && opt.value === value);

                            return (
                                <Chip
                                    key={opt.label}
                                    label={opt.label}
                                    clickable
                                    onClick={() => onChange(opt.value)}
                                    sx={{
                                        fontSize: "0.8rem",
                                        height: 28,

                                        // --- MODIFICA PER CHIP GRADIENTE ---
                                        // Usiamo 'background' invece di 'backgroundColor' 
                                        // perché 'background' supporta i gradienti
                                        background: isSelected
                                            ? accent
                                            : theme.palette.grey[200],

                                        color: isSelected
                                            ? theme.palette.common.white
                                            : theme.palette.text.primary,

                                        // Rimuoviamo il bordo se è selezionato per pulizia visiva col gradiente
                                        border: "none",

                                        "&:hover": {
                                            background: isSelected
                                                ? accent
                                                : theme.palette.grey[300],
                                        },
                                    }}
                                />
                            );
                        })}
                    </Stack>
                </Box>

                <Box sx={{ display: "flex", alignItems: "center" }}>
                    <ChevronRightIcon
                        fontSize="small"
                        sx={{ cursor: "pointer" }}
                        onClick={() => handleScroll("right")}
                    />
                </Box>
            </Box>
        </Box>
    );
};

export default SelectComponent;