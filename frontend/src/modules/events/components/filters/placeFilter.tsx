import React, { useEffect, useState } from "react"
import { Grid, InputLabel, TextField } from "@mui/material";
import { Filters, setPlaceFilter } from "../../store/filtersSlice";
import { useDispatch, useSelector } from "react-redux";

const PlaceFilter = () => {
    const filters = useSelector((state: any) => state.filters as Filters);
    const dispatch = useDispatch();
    const [place, setPlace] = useState<string>('');

    useEffect(() => {
        setPlace(filters.place);
    }, [filters.place])

    const handleChange = (e: any) => {
        dispatch(setPlaceFilter(e.target.value));
    }

    return (
        <Grid item xs={12}>
            <InputLabel component={TextField} value={place} onChange={handleChange} placeholder="Place" sx={{p: 1}}/>
        </Grid>
    )
}

export default PlaceFilter;