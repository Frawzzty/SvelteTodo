<script lang="ts">
    import { onMount } from 'svelte';

    type TodoTask = {
        id: string;
        text: string;
        isDone: boolean;
        doneByDate: string;
    }

    let myTaskText:string = $state("");
    let myTaskDoneDate:string = $state("");

    let tasks = $state<TodoTask[]>([]);

    const baseUrl = "https://localhost:7190";
    const endPoint = "/api/tasks";
    
    //refrence original url that worked 'https://localhost:7190/api/tasks'
    
    onMount(async () => {
        await GetTasksAsync();
    });

    // GET TASKS
    async function GetTasksAsync(){
        try {
            const response = await fetch(baseUrl + endPoint);

            if (!response.ok)
                throw new Error(`Http Error: ${response.status}`);

            tasks = await response.json();
        }
        catch{
            console.log("Failed to load")
        }
    }

    // Create Task
    async function CreateTaskAsync(taskText:string, taskDate:string){
        let newTask:TodoTask = {
            id: "",
            text: taskText,
            isDone: false, // Default to false
            doneByDate: taskDate
        };

        try {
            const response = await fetch(baseUrl + endPoint, {
                method: 'POST',
                headers: {"Content-Type": "application/json"},
                body: JSON.stringify(newTask)
            })

            if (!response.ok)
                throw new Error('Could not add task');

            console.log('Added task successfully');

            // Clear inputs
            ClearNewTaskInputs();

            //tasks.push(newTask) // Add in local list. Not good? as it might not include formating happening in the backend etc
            await GetTasksAsync();
        }
        catch (error){{
            console.error(error);
        }}

    }

    // DELTE Task by ID
    async function DeleteTaskAsync(id:string){
        try {
              const response = await fetch(`${baseUrl}${endPoint}/${id}`, { method: 'DELETE' }
            );
            
            if(!response.ok)
                throw new Error('Could not delete task')

            console.log('Delete task successfully')

            // Remove localy if api managed to delete it;
            tasks = tasks.filter(task => task.id !== id);
        }
        catch (error){
            console.error(error);
        }
    }

    function ClearNewTaskInputs(){
        myTaskText = "";
        myTaskDoneDate = "";
    }
</script>

<main>
    <h1>Svelte Todo</h1>

    <!-- ADD NEW TASK -->
    <div class="addTodoContainer">
        <div>
            <label for="addtodo">New task</label>
        </div>
        <div class="d-flex flex-column flex-gap">
            <input type="text" id="addtodo" class="textInput" bind:value={myTaskText}>
            <input type="Date" bind:value={myTaskDoneDate}/>
            <button onclick={() => CreateTaskAsync(myTaskText, myTaskDoneDate)} class="primaryButton">Add</button>
        </div>
    </div>

    <!-- DISPLAY TASKS -->
    <div class="todoTaskContainer">
        <h2>Tasks</h2>
        {#each tasks as task}
            <article class="todoTask">
                <p>{task.text}</p>
                <p>Done: {task.isDone}</p>
                <p>Date: {task.doneByDate}</p>
                <button class="primaryButton">Update</button>
                <button class="primaryButton" onclick={() => DeleteTaskAsync(task.id)}>Delete</button>
            </article>
        {/each}
       
    </div>
</main>

<style>
    :global(body) {
        background-color: #1e1e2e;
        color: white;
        margin: 0;
        padding: 0;
    }

    main{
        padding: 1rem;
    }

    .d-flex{
        display: flex;
    }

    .flex-column{
        flex-direction: column;
    }
    .flex-gap{
        gap: 0.25rem;
    }
    
    
    .addTodoContainer{
        border: 1px solid #7d7f81;
        padding: 1rem;
        background: #29323b;
        margin-bottom: 1rem;

        border-radius: 5px;
    }

    .todoTaskContainer{
        border: 1px solid #7d7f81;
        border-radius: 5px;
        padding: 1rem;
        background: #29323b;
    }
    .todoTask{
        border: 1px solid #7d7f81;
        background: #3d4a57;
        border-radius: 10px;
        padding: 1rem;
    }

    .primaryButton{
        border-radius: 5px;
        padding: 0.25rem;
        background: #7d7f81;
        color: white;
    }

    .textInput{
        padding-top: 0.5rem;
        padding-bottom: 0.5rem;
    }
</style>