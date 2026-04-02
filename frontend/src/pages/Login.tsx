import LoginForm from "../components/LoginForm";

const Login = () => {
  return (
    <div className="container" style={{ minHeight: 'calc(100vh - 80px)', display: 'flex', alignItems: 'center', justifyContent: 'center', padding: '3rem 0' }}>
      <div className="bg-mesh" style={{ position: 'fixed', inset: 0, zIndex: -1 }} />
      <LoginForm />
    </div>
  );
};

export default Login;