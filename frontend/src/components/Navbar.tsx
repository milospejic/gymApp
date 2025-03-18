import { FC } from "react";
import { Link, useLocation, useNavigate } from "react-router-dom";
import { User, LogOut } from "lucide-react";
import { useAuth } from "../contexts/AuthContext";

const Navbar: FC = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const { isAuthenticated, role, logout } = useAuth();

  const handleLogout = () => {
    logout();
    navigate("/login");
  };

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
            {[
              { path: "/", label: "Home" },
              { path: "/about", label: "About Us" },
              { path: "/plans", label: "Membership Plans" }
            ].map(({ path, label }) => (
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

          {/* Right Side */}
          <ul className="navbar-nav ms-auto mb-2 mb-lg-0">
            {isAuthenticated ? (
              <>
                {/* User Icon with Dropdown */}
                <li className="nav-item dropdown">
                  <Link
                    className="nav-link dropdown-toggle d-flex align-items-center"
                    to="#"
                    id="userDropdown"
                    role="button"
                    data-bs-toggle="dropdown"
                    aria-expanded="false"
                  >
                    <User size={20} className="me-2" />
                  </Link>
                  <ul className="dropdown-menu dropdown-menu-end" aria-labelledby="userDropdown">
                    <li>
                      <Link className="dropdown-item" to="/profile">
                        Profile
                      </Link>
                    </li>
                    {role === "Member" ? (
                      <li>
                        <Link className="dropdown-item" to="/membership">
                          Membership
                        </Link>
                      </li>
                    ) : role === "Admin" ? (
                      <>
                        <li>
                          <Link className="dropdown-item" to="/plan-factory">
                            Plan Factory
                          </Link>
                        </li>
                        <li>
                          <Link className="dropdown-item" to="/dashboard">
                            Dashboard
                          </Link>
                        </li>
                      </>
                    ) : null}
                  </ul>
                </li>

                {/* Logout Button */}
                <li className="nav-item">
                  <button className="btn btn-danger ms-2" onClick={handleLogout}>
                    <LogOut size={20} className="me-1" /> Logout
                  </button>
                </li>
              </>
            ) : (
              <li className="nav-item">
                <Link className="nav-link" to="/login">
                  Login
                </Link>
              </li>
            )}
          </ul>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;