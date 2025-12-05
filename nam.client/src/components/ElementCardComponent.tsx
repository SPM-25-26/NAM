import React from "react";
import { Card, Box, Typography, useTheme } from "@mui/material";

export type ElementCardProps = {
    title: string;
    badge?: string;
    address?: string;
    imageUrl?: string;
    date?: string;
    onClick?: () => void;
};

const ElementCard: React.FC<ElementCardProps> = ({
    title,
    badge,
    address,
    imageUrl,
    date,
    onClick,
}) => {
    const theme = useTheme();

    return (
        // Outer wrapper to emulate gradient border around the card
        <Card
            onClick={onClick}
            sx={{
                // 1. The thickness is set here. The color is transparent to show the gradient below.
                border: "3px solid transparent",

                // 2. Interleave two backgrounds.
                //    - The first is the background color of the card (padding-box)
                //    - The second is the gradient (border-box)
                background: `
                    linear-gradient(${theme.palette.background.default}, ${theme.palette.background.default}) padding-box, 
                    linear-gradient(90deg, rgba(21, 93, 252, 0.50) 0%, rgba(152, 16, 250, 0.50) 100%) border-box
                `,

                borderRadius: "1rem",
                cursor: onClick ? "pointer" : "default",
                boxShadow: "none", // Remove native shadow to avoid dirtying the border
                display: "flex",
                flexDirection: "row",
                alignItems: "stretch",
                position: "relative",
                width: "100%",
                overflow: "hidden",
            }}
        >

            {/* Description badge in the top-right corner */}
            {badge && (
                <Box
                    sx={{
                        position: "absolute",
                        top: 8,
                        right: 8,
                        px: 1.5,
                        py: 0.5,
                        borderRadius: theme.shape.borderRadius,
                        background: "linear-gradient(90deg, rgba(21, 93, 252, 0.50) 0%, rgba(152, 16, 250, 0.50) 100%)",
                        boxShadow: theme.shadows[1],
                    }}
                >
                    <Typography
                        variant="caption"
                        sx={{
                            color: theme.palette.common.white,
                            fontWeight: 600,
                        }}
                    >
                        {badge}
                    </Typography>
                </Box>
            )}

            {/* Left image area */}
            <Box
                sx={{
                    width: 140,
                    minWidth: 140,
                    height: 140,
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "center",
                    padding: 1,
                }}
            >
                {imageUrl ? (
                    <Box
                        component="img"
                        src={imageUrl}
                        alt={title}
                        sx={{
                            width: "100%",
                            height: "100%",
                            objectFit: "cover",
                            borderRadius: "0.75rem",
                        }}
                    />
                ) : (
                    <Typography
                        variant="caption"
                        sx={{
                            color: theme.palette.text.disabled,
                            textAlign: "center",
                            paddingX: 1,
                        }}
                    >
                        No image
                    </Typography>
                )}
            </Box>

            {/* Right text area */}
            <Box
                sx={{
                    flex: 1,
                    padding: 1.5,
                    display: "flex",
                    flexDirection: "column",
                    justifyContent: "center",
                    gap: 0.25,
                }}
            >
                <Typography
                    variant="subtitle1"
                    sx={{
                        fontWeight: 700,
                        color: theme.palette.text.primary,
                        display: "-webkit-box",
                        WebkitLineClamp: 1,
                        WebkitBoxOrient: "vertical",
                        overflow: "hidden",
                    }}
                >
                    {title}
                </Typography>

                {address && (
                    <Typography
                        variant="body2"
                        sx={{
                            color: theme.palette.text.disabled,
                            display: "-webkit-box",
                            WebkitLineClamp: 1,
                            WebkitBoxOrient: "vertical",
                            overflow: "hidden",
                        }}
                    >
                        {address}
                    </Typography>
                )}

                {date && (
                    <Typography
                        variant="caption"
                        sx={{
                            color: theme.palette.text.disabled,
                        }}
                    >
                        {date}
                    </Typography>
                )}
            </Box>
        </Card>
    );
};

export default ElementCard;