import { Paper, BottomNavigation, BottomNavigationAction } from "@mui/material";
import HomeIcon from "@mui/icons-material/Home";
import PersonIcon from "@mui/icons-material/Person";
import { Link, useLocation } from "react-router-dom";
import { AssistantAvatar } from "./assistant/components/AssistantAvatar";
const SimpleBottomNavigation = () => {
  const location = useLocation();

  const getValue = () => {
    if (location.pathname === "/profile") return 2;
    if (location.pathname === "/assistant") return 1;
    return 0;
  };

  return (
    <Paper
      sx={{
        width: "100%",
        zIndex: 1100,
        borderTop: (theme) => `1px solid ${theme.palette.divider}`,
      }}
      elevation={3}
    >
      <BottomNavigation showLabels value={getValue()}>
        <BottomNavigationAction
          label="Home"
          icon={<HomeIcon />}
          component={Link}
          to="/maincontents"
        />
        <BottomNavigationAction
          label={AssistantAvatar.displayName}
          icon={<AssistantAvatar size={32} />}
          component={Link}
          to="/assistant"
        />
        <BottomNavigationAction
          label="Profile"
          icon={<PersonIcon />}
          component={Link}
          to="/profile"
        />
      </BottomNavigation>
    </Paper>
  );
};

export default SimpleBottomNavigation;
