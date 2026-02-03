import { Avatar } from "@mui/material";
import ImgIA from "../../../assets/ia_avatar.webp";

export function AssistantAvatar({ size = 32, animated = false, full = false }) {
  return (
    <Avatar
      src={ImgIA}
      alt="Assistant"
      sx={{
        width: full ? "100%" : size,
        height: full ? "100%" : size,
        ...(animated && {
          animation: "pulse 2s infinite",
        }),
      }}
    />
  );
}

AssistantAvatar.displayName = "Sibilla";
