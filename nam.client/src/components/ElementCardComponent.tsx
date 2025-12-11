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
        <Card
            onClick={onClick}
            sx={{
                border: "3px solid transparent",
                background: `
                    linear-gradient(${theme.palette.background.paper}, ${theme.palette.background.paper}) padding-box, 
                    linear-gradient(90deg, rgba(21, 93, 252, 0.50) 0%, rgba(152, 16, 250, 0.50) 100%) border-box
                `,
                borderRadius: "1rem",
                cursor: onClick ? "pointer" : "default",
                boxShadow: "none",
                display: "flex",
                flexDirection: "column",
                position: "relative",
                width: "100%",
                height: "100%",        // make all cards equal height
                overflow: "hidden",
            }}
        >
            <Box
                sx={{
                    p: 1.2,
                    display: "flex",
                    flexDirection: "column",
                    gap: 1,
                    height: "100%",
                }}
            >
                {/* Image area (16:9, uniform size) */}
                <Box
                    sx={{
                        position: "relative",
                        width: "100%",
                        aspectRatio: "16 / 9",
                        borderRadius: "0.9rem",
                        overflow: "hidden",
                        backgroundColor: theme.palette.action.hover,
                        flexShrink: 0,
                    }}
                >
                    {/* Badge over the image (solid) */}
                    {badge && (
                        <Box
                            sx={{
                                position: "absolute",
                                top: 8,
                                right: 8,
                                px: 1.6,
                                py: 0.7,
                                borderRadius: theme.shape.borderRadius,
                                background:
                                    "linear-gradient(90deg, rgb(138, 174, 254) 0%, rgb(204, 136, 253) 100%)",
                                boxShadow: theme.shadows[2],
                                zIndex: 2,
                            }}
                        >
                            <Typography
                                variant="caption"
                                sx={{
                                    color: theme.palette.common.white,
                                    fontWeight: 700,
                                }}
                            >
                                {badge}
                            </Typography>
                        </Box>
                    )}

                    {imageUrl ? (
                        <Box
                            component="img"
                            src={imageUrl}
                            alt={title}
                            loading="eager"
                            sx={{
                                width: "100%",
                                height: "100%",
                                objectFit: "cover", // switch to "contain" is possible
                                display: "block",
                            }}
                        />
                    ) : (
                        <Box
                            sx={{
                                width: "100%",
                                height: "100%",
                                display: "flex",
                                alignItems: "center",
                                justifyContent: "center",
                                backgroundColor: theme.palette.action.hover,
                            }}
                        >
                            <Typography
                                variant="caption"
                                sx={{
                                    color: theme.palette.text.disabled,
                                    textAlign: "center",
                                    px: 1,
                                }}
                            >
                                No image
                            </Typography>
                        </Box>
                    )}
                </Box>

                {/* Text area under the image */}
                <Box
                    sx={{
                        px: 0.4,
                        pb: 0.2,
                        display: "flex",
                        flexDirection: "column",
                        gap: 0.4,
                        flex: 1, // pushes to fill remaining height so all cards align
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
                                color: theme.palette.text.primary,
                                display: "-webkit-box",
                                WebkitLineClamp: 2,
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
                                color: theme.palette.text.primary,
                            }}
                        >
                            {date}
                        </Typography>
                    )}
                </Box>
            </Box>
        </Card>
    );
};

export default ElementCard;