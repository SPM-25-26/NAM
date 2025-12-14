import { TextField, InputAdornment, Box, InputLabel } from "@mui/material";
import AccountCircle from "@mui/icons-material/AccountCircle";
import "./input_field.css";

interface MyInputFieldProps {
  label: string;
  value: string;
  placeholder: string;
  onChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
  IconComponent?: React.ElementType;
  iconPosition?: "start" | "end";
  variant?: "outlined" | "filled" | "standard";
  isError?: boolean;
  errorMessage?: string;
  type?: string;
}

const MyInputField: React.FC<MyInputFieldProps> = ({
  label = "",
  value = "",
  placeholder = "",
  onChange = () => {},
  IconComponent = AccountCircle,
  iconPosition = "start",
  variant = "outlined",
  isError = false,
  errorMessage = "",
  type = "text",
  ...rest
}) => {
  const Adornment = (
    <InputAdornment position={iconPosition}>
      <IconComponent />
    </InputAdornment>
  );

  return (
    <Box className="input_field_container">
      <InputLabel>{label}</InputLabel>
      <TextField
        className="inputtextfield"
        value={value}
        placeholder={placeholder}
        onChange={onChange}
        variant={variant}
        fullWidth
        margin="none"
        error={isError}
        helperText={isError ? errorMessage : null}
        type={type}
        {...rest}
        InputProps={{
          [iconPosition === "start" ? "startAdornment" : "endAdornment"]:
            Adornment,
        }}
      />
    </Box>
  );
};

export default MyInputField;
