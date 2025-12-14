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
    accentColor?: string;// optional custom color for border and selected chips
}

/**
 * Horizontal scrollable chip selector component with navigation arrows
 */
const SelectComponent: React.FC<SelectComponentProps> = ({
    value,
    options,
    onChange,
    accentColor,
}) => {
    const theme = useTheme();
    const accent = accentColor ?? theme.palette.primary.main;

    // Reference to the scrollable container
    const scrollRef = useRef<HTMLDivElement | null>(null);

    /**
     * Scrolls the chip container left or right
     */
    const handleScroll = (direction: "left" | "right") => {
        const node = scrollRef.current;
        if (!node) return;

        const scrollAmount = 150; // Pixels to scroll per click
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
                border: 3,
                borderColor: accent,
                borderStyle: "solid",
            }}
        >
            {/* Container for scroll arrows and chips */}
            <Box
                sx={{
                    display: "flex",
                    alignItems: "center",
                    gap: 1,
                }}
            >
                {/* Left scroll arrow */}
                <Box
                    sx={{
                        display: "flex",
                        alignItems: "center",
                    }}
                >
                    <ChevronLeftIcon
                        fontSize="small"
                        sx={{ cursor: "pointer" }}
                        onClick={() => handleScroll("left")}
                    />
                </Box>

                {/* Horizontally scrollable chip container */}
                <Box
                    ref={scrollRef}
                    sx={{
                        flex: 1,
                        overflowX: "auto",
                        overflowY: "hidden",
                        // Hide scrollbar across browsers
                        "&::-webkit-scrollbar": {
                            display: "none",
                        },
                        msOverflowStyle: "none", // IE/Edge
                        scrollbarWidth: "none", // Firefox
                    }}
                >
                    <Stack
                        direction="row"
                        spacing={1}
                        sx={{
                            py: 0.5,
                            width: "max-content", // Prevents chips from shrinking
                        }}
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
                                        backgroundColor: isSelected
                                            ? accent
                                            : theme.palette.grey[200],
                                        color: isSelected
                                            ? theme.palette.common.white
                                            : theme.palette.text.primary,
                                        "&:hover": {
                                            backgroundColor: isSelected
                                                ? accent
                                                : theme.palette.grey[300],
                                        },
                                    }}
                                />
                            );
                        })}
                    </Stack>
                </Box>

                {/* Right scroll arrow */}
                <Box
                    sx={{
                        display: "flex",
                        alignItems: "center",
                    }}
                >
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