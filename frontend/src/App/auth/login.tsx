import { Button } from "@mui/material";
import { SyntheticEvent, useState } from "react";
import { useAuth } from 'react-oidc-context';


const Login = () => {
    const [ isSubmitting, setIsSubmitting ] = useState(false);
    const auth = useAuth();

    const signIn = async (event: SyntheticEvent) => {
        event.preventDefault();
        setIsSubmitting(true);
        auth.signinRedirect()
    }

    return (
        <Button onClick={signIn} id="sign-in" color="inherit" disabled={isSubmitting}>Sign In</Button>
    );
}

export default Login;