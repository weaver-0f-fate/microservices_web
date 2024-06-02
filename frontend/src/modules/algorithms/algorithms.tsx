import { Box, Typography } from "@mui/material"
import { useEffect } from "react";
import { useTestAlgorithms } from "./fetch/useTestAlgorithm";


const Algorithms = () => {

    const testAlgorithms = useTestAlgorithms();

    useEffect(() => {
        testAlgorithms().then(result => console.log(result));
    })

    return (
        <Box>
            <Typography>This is Algorithms Page</Typography>
        </Box>
    )
}

export default Algorithms;