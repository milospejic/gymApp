import { FC, useState, useEffect } from "react";
import { Link, useLocation, useNavigate } from "react-router-dom";
import { User, LogOut, Menu, X, Dumbbell, ShieldCheck, LayoutDashboard, UserCircle, CreditCard } from "lucide-react";
import { useAuth } from "../contexts/AuthContext";
import "./Navbar.css";

const Navbar: FC = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const { isAuthenticated, role, logout } = useAuth();
  const [isScrolled, setIsScrolled] = useState(false);
  const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);
  const [isDropdownOpen, setIsDropdownOpen] = useState(false);

  useEffect(() => {
    const handleScroll = () => {
      setIsScrolled(window.scrollY > 20);
    };
    window.addEventListener("scroll", handleScroll);
    return () => window.removeEventListener("scroll", handleScroll);
  }, []);

  const handleLogout = () => {
    logout();
    navigate("/login");
  };

  const navLinks = [
    { path: "/", label: "Home" },
    { path: "/about", label: "About" },
    { path: "/plans", label: "Plans" }
  ];

  return (
    <nav className={`navbar ${isScrolled ? "navbar-scrolled" : ""}`}>
      <div className="container navbar-inner">
        
        {/* Logo */}
        <Link to="/" className="navbar-logo">
          <div className="logo-icon">
            <Dumbbell className="icon-white" />
          </div>
          <span className="logo-text italic">
            My<span className="text-primary">Gym</span>
          </span>
        </Link>

        {/* Desktop Navigation */}
        <div className="navbar-desktop">
          <div className="nav-links">
            {navLinks.map(({ path, label }) => (
              <Link
                key={path}
                to={path}
                className={`nav-link ${location.pathname === path ? "nav-link-active" : ""}`}
              >
                {label}
              </Link>
            ))}
          </div>

          <div className="nav-divider" />

          <div className="nav-actions">
            {isAuthenticated ? (
              <div className="nav-user-dropdown">
                <button 
                  onClick={() => setIsDropdownOpen(!isDropdownOpen)}
                  onBlur={() => setTimeout(() => setIsDropdownOpen(false), 200)}
                  className="user-button"
                >
                  <UserCircle className="text-primary" size={20} />
                  <span className="user-role">{role}</span>
                </button>

                {isDropdownOpen && (
                  <div className="dropdown-menu animate-fade-in">
                    <Link to="/profile" className="dropdown-item">
                      <User size={16} /> Profile
                    </Link>
                    {role === "Member" && (
                      <Link to="/membership" className="dropdown-item">
                        <CreditCard size={16} /> Membership
                      </Link>
                    )}
                    {role === "Admin" && (
                      <>
                        <Link to="/plan-factory" className="dropdown-item">
                          <ShieldCheck size={16} /> Plan Factory
                        </Link>
                        <Link to="/dashboard" className="dropdown-item">
                          <LayoutDashboard size={16} /> Dashboard
                        </Link>
                      </>
                    )}
                    <div className="dropdown-divider" />
                    <button 
                      onClick={handleLogout}
                      className="dropdown-item logout-button"
                    >
                      <LogOut size={16} /> Logout
                    </button>
                  </div>
                )}
              </div>
            ) : (
              <Link to="/login" className="btn-premium">
                Sign In
              </Link>
            )}
          </div>
        </div>

        {/* Mobile Menu Toggle */}
        <div className="navbar-mobile-toggle">
          <button 
            onClick={() => setIsMobileMenuOpen(!isMobileMenuOpen)}
            className="toggle-button"
          >
            {isMobileMenuOpen ? <X size={24} /> : <Menu size={24} />}
          </button>
        </div>
      </div>

      {/* Mobile Menu */}
      {isMobileMenuOpen && (
        <div className="mobile-menu animate-fade-in">
          {navLinks.map(({ path, label }) => (
            <Link
              key={path}
              to={path}
              onClick={() => setIsMobileMenuOpen(false)}
              className={`mobile-nav-link ${location.pathname === path ? "nav-link-active" : ""}`}
            >
              {label}
            </Link>
          ))}
          <div className="dropdown-divider" />
          {isAuthenticated ? (
            <div className="mobile-user-actions">
              <Link 
                to="/profile" 
                onClick={() => setIsMobileMenuOpen(false)}
                className="mobile-nav-link"
              >
                Profile
              </Link>
              <button 
                onClick={handleLogout}
                className="btn-premium logout-btn"
              >
                Logout
              </button>
            </div>
          ) : (
            <Link 
              to="/login" 
              onClick={() => setIsMobileMenuOpen(false)}
              className="btn-premium mobile-signin-btn"
            >
              Sign In
            </Link>
          )}
        </div>
      )}
    </nav>
  );
};

export default Navbar;