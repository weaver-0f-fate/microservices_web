import { Box, CircularProgress } from '@mui/material';
import ErrorOutlineIcon from '@mui/icons-material/ErrorOutline';
import { LoadingState } from '../../shared/constants/loadingState';

export type LoadingBackgroundInput = {
    loadingState: LoadingState
}

const LoadingBackground = (props: LoadingBackgroundInput) => {
    return (
        <Box sx={{ 
            height: '100vh', 
            width:'100vw',
            display:'flex', 
            alignItems: 'center', 
            justifyContent: 'center'
        }}>
            { props.loadingState === LoadingState.Loading && <CircularProgress sx={{ mt: 3}} /> }
            { props.loadingState === LoadingState.Error &&  <ErrorOutlineIcon color="error" /> }
        </Box>
    )
}

export default LoadingBackground;