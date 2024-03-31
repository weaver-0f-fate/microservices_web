import React, { useEffect, useState } from "react"
import { Grid, InputLabel, TextField } from "@mui/material";
import { Filters, setCategoryFilter } from "../../store/filtersSlice";
import { useDispatch, useSelector } from "react-redux";

const CategoryFilter = () => {
    const filters = useSelector((state: any) => state.filters as Filters);
    const dispatch = useDispatch();
    const [category, setCategory] = useState<string>('');

    useEffect(() => {
        setCategory(filters.category);
    }, [filters.category])

    const handleChange = (e: any) => {
        dispatch(setCategoryFilter(e.target.value));
    }

    return (
        <Grid item xs={12}>
            <InputLabel component={TextField} value={category} onChange={handleChange} placeholder="Category" sx={{p:1}} />
        </Grid>
    )
}

export default CategoryFilter;