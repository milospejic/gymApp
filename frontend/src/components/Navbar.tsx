import { FC } from "react";
import { Link, useLocation } from "react-router-dom";

const Navbar: FC = () => {
  const location = useLocation();

  return (
    <nav className="navbar navbar-expand-lg navbar-primary shadow-sm">
      <div className="container-fluid">
        <Link className="navbar-brand fs-2 fw-bold" to="/">
          MyGym
        </Link>

        <button
          className="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarSupportedContent"
          aria-controls="navbarSupportedContent"
          aria-expanded="false"
          aria-label="Toggle navigation"
        >
          <span className="navbar-toggler-icon"></span>
        </button>

        <div className="collapse navbar-collapse" id="navbarSupportedContent">
        <ul className="navbar-nav mb-2 mb-lg-0 text-center">
            {/* Left side links */}
            {[{ path: "/", label: "Home" }, { path: "/about", label: "About Us" }, { path: "/plans", label: "Membership Plans" }].map(({ path, label }) => (
              <li className="nav-item" key={path}>
                <Link
                  className={`nav-link ${location.pathname === path ? "active fw-bold text-primary" : ""}`}
                  to={path}
                >
                  {label}
                </Link>
              </li>
            ))}
          </ul>
          {/* Right side link */}
          <ul className="navbar-nav ms-auto mb-2 mb-lg-0">
            <li className="nav-item">
              <Link className="nav-link" to="/login">
                Login
              </Link>
            </li>
          </ul>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
