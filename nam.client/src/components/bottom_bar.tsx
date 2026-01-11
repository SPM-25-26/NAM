import { Paper, BottomNavigation, BottomNavigationAction } from "@mui/material";
import HomeIcon from "@mui/icons-material/Home";
import PersonIcon from "@mui/icons-material/Person";
import { Link, useLocation } from "react-router-dom"; // Importa Link da qui

const SimpleBottomNavigation = () => {
  const location = useLocation();

  // Invece di usare uno stato interno, leggiamo il path attuale.
  // Così se l'utente ricarica la pagina su /profile, l'icona corretta resta accesa.
  const getValue = () => {
    if (location.pathname === "/profile") return 1;
    return 0;
  };

  return (
    <Paper
      sx={{
        position: "fixed",
        bottom: 0,
        left: 0,
        right: 0,
        zIndex: 1100,
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
