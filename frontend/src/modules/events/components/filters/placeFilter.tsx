import React, { useEffect, useState } from "react"
import useSetPlaceFilter from "../../store/hooks/filters/useSetPlaceFilter";
import { Grid, InputLabel, TextField } from "@mui/material";
import useEvent from "../../store/hooks/useEvent";

const PlaceFilter = () => {
    const setPlaceFilter = useSetPlaceFilter();
    const eventStore = useEvent();
    const [place, setPlace] = useState<string>('');

    useEffect(() => {
        setPlace(eventStore.filters.place);
    }, [eventStore.filters.place])

    const handleChange = (e: any) => {
        setPlaceFilter(e.target.value);
    }

    return (
        <Grid item xs={12}>
            <InputLabel component={TextField} value={place} onChange={handleChange} placeholder="Place" sx={{p: 1}}/>
        </Grid>
    )
}

export default PlaceFilter;