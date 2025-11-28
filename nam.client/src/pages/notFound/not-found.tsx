import { Link, useLocation } from "react-router-dom";

function NotFoundPage() {
  const location = useLocation();

  return (
    <div style={{ textAlign: "center", padding: "50px" }}>
      <h1>404</h1>
      <h2>Oops! Page not found</h2>

      <p style={{ fontSize: "1.1em", color: "#666" }}>
        We are sorry, we cannot find the route:
        <code
          style={{
            backgroundColor: "#eee",
            padding: "2px 5px",
            borderRadius: "4px",
          }}
        >
          {location.pathname}
        </code>
      </p>

      <p>
        <Link
          to="/"
          style={{ textDecoration: "none", color: "teal", fontWeight: "bold" }}
        >
          Go Home
        </Link>
      </p>
    </div>
  );
}

export default NotFoundPage;
