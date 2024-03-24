import React, { useEffect, useState } from "react"
import useSetCategoryFilter from "../../store/hooks/filters/useSetCategoryFilter";
import { Grid, InputLabel, TextField } from "@mui/material";
import useEvent from "../../store/hooks/useEvent";

const CategoryFilter = () => {
    const setCategoryFilter = useSetCategoryFilter();
    const eventStore = useEvent();
    const [category, setCategory] = useState<string>('');

    useEffect(() => {
        setCategory(eventStore.filters.category);
    }, [eventStore.filters.category])

    const handleChange = (e: any) => {
        setCategoryFilter(e.target.value);
    }

    return (
        <Grid item xs={12}>
            <InputLabel component={TextField} value={category} onChange={handleChange} placeholder="Category" sx={{p:1}} />
        </Grid>
    )
}

export default CategoryFilter;