import { createContext, useContext, useMemo, useState } from "react";

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [token, setToken] = useState(() => localStorage.getItem("token"));

  const value = useMemo(
    () => ({
      token,
      isAuthenticated: !!token,
      login: (t) => { localStorage.setItem("token", t); setToken(t); },
      logout: () => { localStorage.removeItem("token"); setToken(null); },
    }),
    [token]
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export const useAuth = () => {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth must be used inside AuthProvider");
  return ctx;
};