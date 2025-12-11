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
    accentColor?: string; // optional custom color for border and selected chips
}

const SelectComponent: React.FC<SelectComponentProps> = ({
    value,
    options,
    onChange,
    accentColor,
}) => {
    const theme = useTheme();
    const accent = accentColor ?? theme.palette.primary.main;

    // Ref to the horizontal scroll container
    const scrollRef = useRef<HTMLDivElement | null>(null);

    const handleScroll = (direction: "left" | "right") => {
        const node = scrollRef.current;
        if (!node) return;

        const delta = 150; // how many pixels to scroll per click
        const newScrollLeft =
            direction === "right"
                ? node.scrollLeft + delta
                : node.scrollLeft - delta;

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
            {/* Container for arrows + horizontally scrollable chips */}
            <Box
                sx={{
                    display: "flex",
                    alignItems: "center",
                    gap: 1,
                }}
            >
                {/* Left arrow */}
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

                {/* Scrollable chip row */}
                <Box
                    ref={scrollRef}
                    sx={{
                        flex: 1,
                        overflowX: "auto",
                        overflowY: "hidden",
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
                            width: "max-content", // prevents chips from shrinking
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

                {/* Right arrow */}
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