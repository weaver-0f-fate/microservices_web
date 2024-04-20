import { Box } from "@mui/material";
import React from "react";

interface ContentProps {
    children: React.ReactNode;
}

const Content = (props: ContentProps) => {
    const { children } = props;

    return (
        <Box display='flex' flexGrow={1}>
            {children}
        </Box>
    )
}

export default Content;