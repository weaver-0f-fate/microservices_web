import { Button } from "@mui/material";
import { useCallback, useState } from "react";
import { useAuth } from 'react-oidc-context';


const Login = () => {
    const [isSubmitting, setIsSubmitting] = useState(false);
    const auth = useAuth();

    const logout = useCallback(async () => {
        setIsSubmitting(true);
        await auth.signoutSilent({ post_logout_redirect_uri: window.location.origin });
    }, [auth]);

    return (
        <Button onClick={logout} id="logout" color="inherit" disabled={isSubmitting}>Logout</Button>
    );
}

export default Login;